using UnityEngine;
using System.Collections.Generic;

public class CheckService
{
    private GridSystem grid;

    public CheckService(GridSystem grid)
    {
        this.grid = grid;
    }

    public bool IsKingInCheck(Teams.TeamName team)
    {
        ChessPiece king = FindKing(team);
        if (king == null)
        {
            Debug.LogError("Rey no encontrado para el equipo " + team);
            return false;
        }

        Vector2Int kingPos = king.position;
        
        foreach (ChessPiece piece in grid.GetAllPieces())
        {
            if (piece.team == team)
                continue;

            if (piece.isMovementLegal(piece, kingPos))
                return true;
        }

        return false;
    }

    private ChessPiece FindKing(Teams.TeamName team)
    {
        foreach (ChessPiece piece in grid.GetAllPieces())
        {
            if (piece.team == team && piece.pieceType == PieceType.Type.King)
                return piece;
        }

        return null;
    }

    public bool WouldMoveGiveCheck(
    ChessPiece piece,
    Vector2Int from,
    Vector2Int to,
    Teams.TeamName enemyTeam
)
    {
        // Guardar estado
        ChessPiece capturedPiece = null;
        bool wasOccupied = grid.IsCellOccupied(to);

        if (wasOccupied)
        {
            capturedPiece = grid.GetPieceOnGridPosition(to);
            capturedPiece.gameObject.SetActive(false);
        }

        grid.SetCellAsOccupied(from, false);
        piece.SetPosition(to);
        grid.SetCellAsOccupied(to, true);

        // Checkear jaque
        bool givesCheck = IsKingInCheck(enemyTeam);

        // Revertir estado
        piece.SetPosition(from);
        grid.SetCellAsOccupied(from, true);
        grid.SetCellAsOccupied(to, wasOccupied);

        if (capturedPiece != null)
            capturedPiece.gameObject.SetActive(true);

        return givesCheck;
    }

    public bool WouldLeaveKingInCheck(
        ChessPiece piece,
        Vector2Int from,
        Vector2Int to
    )
    {
        ChessPiece capturedPiece = null;
        bool wasOccupied = grid.IsCellOccupied(to);

        if (wasOccupied)
        {
            capturedPiece = grid.GetPieceOnGridPosition(to);
            capturedPiece.gameObject.SetActive(false);
        }

        // Simular
        grid.SetCellAsOccupied(from, false);
        piece.SetPosition(to);
        grid.SetCellAsOccupied(to, true);

        bool inCheck = IsKingInCheck(piece.team);

        // Revertir
        piece.SetPosition(from);
        grid.SetCellAsOccupied(from, true);
        grid.SetCellAsOccupied(to, wasOccupied);

        if (capturedPiece != null)
            capturedPiece.gameObject.SetActive(true);

        return inCheck;
    }
}
