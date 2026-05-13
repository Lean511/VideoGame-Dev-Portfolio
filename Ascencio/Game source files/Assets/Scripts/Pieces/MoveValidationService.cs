using UnityEngine;

public class MoveValidationService
{
    private GridSystem grid;
    private CheckService checkService;

    public MoveValidationService(GridSystem grid, CheckService checkService)
    {
        this.grid = grid;
        this.checkService = checkService;
    }

    public bool IsMoveLegalConsideringCheck(
        ChessPiece piece,
        Vector2Int target
    )
    {
        Vector2Int originalPos = piece.position;
        ChessPiece capturedPiece = null;

        // Simular captura
        if (grid.IsCellOccupied(target))
        {
            capturedPiece = grid.GetPieceOnGridPosition(target);
            capturedPiece.gameObject.SetActive(false);
        }

        // Simular movimiento
        piece.position = target;
        grid.SetCellAsOccupied(originalPos, false);
        grid.SetCellAsOccupied(target, true);

        bool kingInCheck = checkService.IsKingInCheck(piece.team);

        //Deshacer simulación
        piece.position = originalPos;
        grid.SetCellAsOccupied(originalPos, true);
        grid.SetCellAsOccupied(target, capturedPiece != null);

        if (capturedPiece != null)
            capturedPiece.gameObject.SetActive(true);

        return !kingInCheck;
    }
}
