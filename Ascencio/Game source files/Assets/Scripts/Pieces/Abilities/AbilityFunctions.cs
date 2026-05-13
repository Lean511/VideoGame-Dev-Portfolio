using UnityEngine;

public class AbilityFunctions : MonoBehaviour
{
    //Aquí se especifican los requisitos para ejecutar cada habilidad.
    public bool CheckAbilityFunction(int id, ChessPiece piece, Vector2Int destination)
    {
        switch (id)
        {
            case 0:
                //Requirements for ability with id 0.
                //Castle
                ChessPiece selectedPiece = piece.GetGridSystem().GetPieceOnGridPosition(destination);
                if (!piece.hasMoved && !selectedPiece.hasMoved)
                {
                    return true;
                }
                break;
            case 1:
                //Requirements for ability with id 1.
                break;
        }

        return false; //Returns false by default if the ability can't be executed.
    }

    //Aquí se especifican las acciones que realiza cada habilidad.
    public void ActivateAbilityFunction(int id, ChessPiece casterPiece, Vector2Int destination)
    {
        //Deselecciona la habilidad después de usarla.
        casterPiece.selectedAbility = 0;

        switch (id)
        {
            case 0:
                //Function for ability with id 0.
                //Castle
                ChessPiece king = casterPiece.GetGridSystem().GetPieceOnGridPosition(destination);

                //Si la posición del rey es menor a la de la torre, es enroque corto.
                if (king.GetHorizontalPosition() < casterPiece.GetHorizontalPosition())
                {
                    casterPiece.MovePiece(new Vector2Int((king.GetHorizontalPosition() + 1), king.GetVerticalPosition()), null);
                    king.MovePiece(new Vector2Int((king.GetHorizontalPosition() + 2), king.GetVerticalPosition()), null);
                }
                else //Enroque largo.
                {
                    casterPiece.MovePiece(new Vector2Int((king.GetHorizontalPosition() - 1), king.GetVerticalPosition()), null);
                    king.MovePiece(new Vector2Int((king.GetHorizontalPosition() - 2), king.GetVerticalPosition()), null);
                }
                break;
            case 1:
                //Function for ability with id 1.
                break;
        }
    }
}
