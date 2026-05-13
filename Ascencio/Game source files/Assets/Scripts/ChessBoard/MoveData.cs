using UnityEngine;
[System.Serializable]
public class MoveData
{
    public ChessPiece piece;
    public Vector2Int from;
    public Vector2Int to;
    public int turn;

    public ChessPiece capturedPiece;
    public Teams.TeamName team;
}
