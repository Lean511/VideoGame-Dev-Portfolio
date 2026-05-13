using UnityEngine;

public class BoardPiece
{
    public PieceType.Type type;
    public Teams.TeamName team;
    public Vector2Int position;

    public BoardPiece(
        PieceType.Type type,
        Teams.TeamName team,
        Vector2Int position)
    {
        this.type = type;
        this.team = team;
        this.position = position;
    }
}