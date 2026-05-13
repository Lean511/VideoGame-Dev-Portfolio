using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void OnPieceCaptured(ChessPiece capturedPiece)
    {
        if (gameEnded)
            return;

        if (capturedPiece.pieceType == PieceType.Type.King)
        {
            EndGame(capturedPiece.team);
        }
    }

    public void EndGame(Teams.TeamName defeatedTeam)
    {
        gameEnded = true;

        Teams.TeamName winner =
            defeatedTeam == Teams.TeamName.White
            ? Teams.TeamName.Black
            : Teams.TeamName.White;

        Debug.Log($"GANÓ {winner} – El rey fue capturado");

        // Despues hay que hacer:
        // - bloquear inputs
        // - mostrar UI
        // - botón de reinicio
    }
}
