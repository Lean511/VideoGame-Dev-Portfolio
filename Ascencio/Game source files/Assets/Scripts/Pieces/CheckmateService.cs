using UnityEngine;
using System.Collections.Generic;

public class CheckmateService
{
    private GridSystem grid;
    private CheckService checkService;
    private MoveValidationService moveValidationService;

    public CheckmateService(
        GridSystem grid,
        CheckService checkService,
        MoveValidationService moveValidationService)
    {
        this.grid = grid;
        this.checkService = checkService;
        this.moveValidationService = moveValidationService;
    }

    public bool IsCheckmate(Teams.TeamName team)
    {
        if (!checkService.IsKingInCheck(team))
            return false;

        foreach (ChessPiece piece in grid.GetAllPieces())
        {
            if (piece.team != team)
                continue;

            for (int x = 0; x < grid.width; x++)
            {
                for (int y = 0; y < grid.height; y++)
                {
                    Vector2Int target = new Vector2Int(x, y);

                    if (!piece.isMovementLegal(piece, target))
                        continue;

                    if (moveValidationService
                        .IsMoveLegalConsideringCheck(piece, target))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public bool WouldMoveBeCheckmate(
    ChessPiece piece,
    Vector2Int from,
    Vector2Int to,
    Teams.TeamName enemyTeam
)
    {
        // Simular movimiento
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

        // Verificar jaque mate
        bool isMate = IsCheckmate(enemyTeam);

        // Revertir
        piece.SetPosition(from);
        grid.SetCellAsOccupied(from, true);
        grid.SetCellAsOccupied(to, wasOccupied);

        if (capturedPiece != null)
            capturedPiece.gameObject.SetActive(true);

        return isMate;
    }
}
