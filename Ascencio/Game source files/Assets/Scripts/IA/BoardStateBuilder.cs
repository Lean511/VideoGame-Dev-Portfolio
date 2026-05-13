using UnityEngine;

public static class BoardStateBuilder
{
    public static BoardState FromGrid(GridSystem grid)
    {
        BoardState state = new BoardState(grid.width, grid.height);

        ChessPiece[] pieces = GameObject.FindObjectsByType<ChessPiece>(FindObjectsSortMode.None);

        foreach (ChessPiece piece in pieces)
        {
            Vector2Int pos = piece.position;

            if (!state.IsInside(pos))
                continue;

            state.board[pos.x, pos.y] = new BoardPiece(
                piece.pieceType,
                piece.team,
                pos
            );
        }

        return state;
    }
}