using UnityEngine;

public class PieceTeamIndicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer indicatorRenderer;
    [SerializeField] private Color whiteColor = Color.white;
    [SerializeField] private Color blackColor = Color.black;

    private ChessPiece piece;

    private void Awake()
    {
        piece = GetComponentInParent<ChessPiece>();
        indicatorRenderer = GetComponent<SpriteRenderer>();

        indicatorRenderer.sortingLayerName = "Indicator";
        indicatorRenderer.sortingOrder = 1;
    }

    private void Start()
    {
        if (piece == null)
        {
            Debug.LogError("[PieceTeamIndicator] ChessPiece NO encontrada");
            return;
        }
        UpdateIndicator();
    }

    public void UpdateIndicator()
    {
        if (piece.team == Teams.TeamName.White)
            indicatorRenderer.color = new Color(0.9f, 0.9f, 0.9f);
        else
            indicatorRenderer.color = new Color(0.15f, 0.15f, 0.15f);
    }
}