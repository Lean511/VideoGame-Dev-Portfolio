using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    public int width;
    public int height;

    // Estado puro del tablero
    public BoardPiece[,] board;

    public BoardState(int width, int height)
    {
        this.width = width;
        this.height = height;
        board = new BoardPiece[width, height];
    }

    public BoardPiece GetPiece(Vector2Int pos)
    {
        if (!IsInside(pos)) return null;
        return board[pos.x, pos.y];
    }

    public bool IsInside(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
    }
}