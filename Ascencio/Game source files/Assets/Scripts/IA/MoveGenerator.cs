using System.Collections.Generic;
using UnityEngine;

public static class MoveGenerator
{
    public static List<AIMoveData> GenerateMoves(
        BoardState state,
        Teams.TeamName team)
    {
        List<AIMoveData> moves = new List<AIMoveData>();

        for (int x = 0; x < state.width; x++)
        {
            for (int y = 0; y < state.height; y++)
            {
                BoardPiece piece = state.board[x, y];
                if (piece == null) continue;
                if (piece.team != team) continue;

                Vector2Int from = new Vector2Int(x, y);

                switch (piece.type)
                {
                    case PieceType.Type.Pawn:
                        GeneratePawnMoves(state, piece, from, moves);
                        break;
                }
            }
        }

        return moves;
    }

    private static void GeneratePawnMoves(
        BoardState state,
        BoardPiece piece,
        Vector2Int from,
        List<AIMoveData> moves)
    {
        int dir = piece.team == Teams.TeamName.White ? 1 : -1;

        Vector2Int forward = new Vector2Int(from.x, from.y + dir);

        // Movimiento simple
        if (state.IsInside(forward) && state.GetPiece(forward) == null)
        {
            moves.Add(new AIMoveData(from, forward));
        }

        // Capturas
        Vector2Int[] diagonals =
        {
            new Vector2Int(from.x - 1, from.y + dir),
            new Vector2Int(from.x + 1, from.y + dir)
        };

        foreach (Vector2Int target in diagonals)
        {
            if (!state.IsInside(target)) continue;

            BoardPiece enemy = state.GetPiece(target);
            if (enemy != null && enemy.team != piece.team)
            {
                moves.Add(new AIMoveData(from, target));
            }
        }
    }
}