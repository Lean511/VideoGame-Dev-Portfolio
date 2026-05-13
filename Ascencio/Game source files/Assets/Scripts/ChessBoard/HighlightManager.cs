using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    [Header("Alturas de highlights")]
    public float moveHighlightY = 0.01f;
    public float lastMoveHighlightY = 0.005f;
    public float pastMoveHighlightY = 0.008f;

    public GameObject highlightPrefab;
    public GridSystem gridSystem;

    private List<GameObject> moveHighlights = new();
    private List<GameObject> pastMoveHighlights = new();
    private List<GameObject> lastMoveHighlights = new();

    public float highlightHeightOffset = 0.01f;



    void Awake()
    {
        var gridSystem = FindObjectsByType<GridSystem>(FindObjectsSortMode.None);
        if (gridSystem == null)
        {
            Debug.LogError("HighlightManager: No se encontró GridSystem en la escena.");
        }
    }

    // Muestra los highlights de una pieza seleccionada
    public void ShowHighlights(ChessPiece piece)
    {
        /////////////////////
        if (GameManager.Instance != null && GameManager.Instance.gameEnded)
            return;
        /////////////////////////
        ClearLastMoveHighlights();
        ClearMoveHighlights();

        foreach (var move in GetPosibleMoves(piece))
        {
            SpawnHighlight(
            move,
            GetRarityColor(piece),
            moveHighlights,
            moveHighlightY
        );
        }
    }

    // Devuelve las direcciones según la pieza
    private List<Vector2Int> GetPosibleMoves(ChessPiece piece)
    {
        PieceType.Type type = piece.pieceType;
        List<Vector2Int> directions = new List<Vector2Int>();

        for (int i = 0; i < gridSystem.width; i++)
        {
            for (int j = 0; j < gridSystem.height; j++)
            {
                //Debug.Log("Debug: Checking highlights for cell" + (new Vector2Int(i, j)));
                if (piece.isMovementLegal(piece, new Vector2Int(i, j)))
                {
                    //if (gridSystem.GetPieceOnGridPosition(new Vector2Int(i, j)).team != piece.team || !gridSystem.IsCellOccupied(new Vector2Int(i, j)))
                    //{
                        //Debug.Log("Debug: Valid highlight for cell" + (new Vector2Int(i, j)));
                        directions.Add(new Vector2Int(i, j));
                    //}
                }
            }
        }
        return directions;
    }

    public void ShowLastMove(Teams.TeamName currentPlayerTeam)
    {
        if (gridSystem.lastMoveFrom != Vector2Int.zero)
            SpawnHighlight(
                gridSystem.lastMoveFrom,
                new Color(0.4f, 0.4f, 0.4f, 1f),
                lastMoveHighlights,
                lastMoveHighlightY
            );

        if (gridSystem.lastMoveTo != Vector2Int.zero)
            SpawnHighlight(
                gridSystem.lastMoveTo,
                new Color(0.4f, 0.4f, 0.4f, 1f),
                lastMoveHighlights,
                lastMoveHighlightY
            );
    }

    private void SpawnHighlight(
    Vector2Int gridPos,
    Color color,
    List<GameObject> list,
    float height
)
    {
        Vector3 worldPos = gridSystem.GetWorldPosition(gridPos.x, gridPos.y);
        worldPos.y = height;

        GameObject h = Instantiate(highlightPrefab, worldPos, Quaternion.identity);

        Renderer r = h.GetComponent<Renderer>();
        if (r != null)
            r.material.color = color;

        list.Add(h);
    }

    public void ShowPastMoves(ChessPiece piece, MoveHistory history)
    {
        ClearPastMoveHighlights();

        foreach (var move in history.GetMovesForPiece(piece))
        {
            SpawnHighlight(move.from, new Color(0, 1, 1, 0.6f), pastMoveHighlights, pastMoveHighlightY);
            SpawnHighlight(move.to, new Color(0, 1, 1, 0.6f), pastMoveHighlights, pastMoveHighlightY);
        }
    }

    public void ClearMoveHighlights() => ClearList(moveHighlights);
    private void ClearPastMoveHighlights() => ClearList(pastMoveHighlights);
    private void ClearLastMoveHighlights() => ClearList(lastMoveHighlights);

    private void ClearList(List<GameObject> list)
    {
        foreach (var h in list) Destroy(h);
        list.Clear();
    }

    private Color GetRarityColor(ChessPiece piece)
    {
        switch (piece.rarity)
        {
            case ItemClass.ClassName.Common: return Color.white;
            case ItemClass.ClassName.Rare: return Color.blue;
            case ItemClass.ClassName.Epic: return new Color(0.5f, 0, 0.5f);
            case ItemClass.ClassName.Legendary: return Color.yellow;
            case ItemClass.ClassName.Mythic: return Color.red;
            default: return Color.white;
        }
    }
}