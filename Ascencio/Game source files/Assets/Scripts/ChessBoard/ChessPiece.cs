using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessPiece : Piece
{
    [SerializeField] private GridSystem gridSystem;
    public Vector2Int position;
    public bool isSelected;
    public bool hasMoved = false;///////
    public Teams.TeamName team;
    public int playerId; //0: Jugador, 1: Rival
    public int selectedAbility = 0;
    // Remove the field initializer for inputManager and initialize it in Awake instead.
    public InputManager inputManager;
    public void Eat()
    {
        GameManager.Instance.OnPieceCaptured(this);

        gridSystem.DespawnPiece(this);
        Debug.Log("Pieza " + pieceName + " comida.");
    }

    public void SetPosition(Vector2Int positionOnGrid)
    {
        // Usar el método del GridSystem para obtener la posición exacta
        transform.position = gridSystem.GetWorldPosition(positionOnGrid.x, positionOnGrid.y);
        this.position = positionOnGrid;
    }

    public void SetGridSystem(GridSystem gridSystem)
    {
        this.gridSystem = gridSystem;
    }

    private void OnDrawGizmos()
    {
        if (isSelected)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(this.transform.position, Vector3.one * 0.6f);
        }
    }

    public bool isMovementLegal(ChessPiece piece, Vector2Int destination)
    {
        if (gridSystem.IsCellOccupied(destination))
        {
            //El movimiento se convierte en ilegal si la celda de destino está ocupada por una pieza del mismo equipo.
            if (gridSystem.GetPieceOnGridPosition(destination).team == piece.team)
            {
                //if (piece.abilityGroup >= 0 && piece.GetAbilityGroup().DoesAnyAbilityTargetsSameTeam(piece))
                //Si la pieza tiene habilidades que pueden afectar a piezas del mismo equipo, se crea una excepción.
                if (piece.abilityGroup >= 0 && piece.GetAbilityGroup().CheckAbilities(piece, destination))
                {
                    selectedAbility = piece.GetAbilityGroup().GetActivableAbilityId(piece, destination);
                }
                else
                {
                    return false;
                }
            }
        }

        //Para que la IA no se mueva al mismo lugar
        if (destination == piece.position)
            return false;

        //Por si la IA intenta salirse del tablero
        if (!gridSystem.IsInside(destination.x, destination.y))
            return false;

        //Obtiene la diferencia entre la posición actual y la de destino
        Vector2Int move = destination - piece.position;

        //Convierte los valores negativos en positivos para que no hayan movimientos en negativo.
        if (move.x < 0)
        {
            move.x = move.x * -1;
        }
        if (move.y < 0)
        {
            move.y = move.y * -1;
        }

        //Según el tipo de pieza, se asignan los valores correspondientes a sus movimientos permitidoss
        switch (piece.pieceType)
        {
            case PieceType.Type.Pawn:
                {
                    int direction = (piece.team == Teams.TeamName.White) ? 1 : -1;

                    Vector2Int oneForward = new Vector2Int(piece.position.x, piece.position.y + direction);
                    Vector2Int twoForward = new Vector2Int(piece.position.x, piece.position.y + direction * 2);

                    // Avanzar 1
                    if (destination == oneForward && !gridSystem.IsCellOccupied(destination))
                        return true;

                    // Avanzar 2 (solo si no se movió)
                    if (!piece.hasMoved &&
                        destination == twoForward &&
                        !gridSystem.IsCellOccupied(oneForward) &&
                        !gridSystem.IsCellOccupied(twoForward))
                        return true;

                    // Comer en diagonal
                    Vector2Int diagLeft = new Vector2Int(piece.position.x - 1, piece.position.y + direction);
                    Vector2Int diagRight = new Vector2Int(piece.position.x + 1, piece.position.y + direction);

                    if (destination == diagLeft || destination == diagRight)
                    {
                        if (gridSystem.IsCellOccupied(destination))
                        {
                            ChessPiece target = gridSystem.GetPieceOnGridPosition(destination);
                            if (target.team != piece.team)
                                return true;
                        }
                    }

                    return false;
                }
            case PieceType.Type.Rook:
                canFly = false;
                minHorizontalMoves = 1;
                maxHorizontalMoves = int.MaxValue;
                minVerticalMoves = 1;
                maxVerticalMoves = int.MaxValue;
                minDiagonalMoves = -1;
                maxDiagonalMoves = -1;
                break;
            case PieceType.Type.Bishop:
                canFly = false;
                minHorizontalMoves = -1;
                maxHorizontalMoves = -1;
                minVerticalMoves = -1;
                maxVerticalMoves = -1;
                minDiagonalMoves = 1;
                maxDiagonalMoves = int.MaxValue;
                break;
            case PieceType.Type.Knight:
                canFly = true;

                if ((move.x == 1 && move.y == 2) || (move.x == 2 && move.y == 1))
                    return true;

                return false;
            case PieceType.Type.King:
                canFly = false;
                minHorizontalMoves = 1;
                maxHorizontalMoves = 1;
                minVerticalMoves = 1;
                maxVerticalMoves = 1;
                minDiagonalMoves = 1;
                maxDiagonalMoves = 1;
                break;
            case PieceType.Type.Queen:
                canFly = false;
                minHorizontalMoves = 1;
                maxHorizontalMoves = int.MaxValue;
                minVerticalMoves = 1;
                maxVerticalMoves = int.MaxValue;
                minDiagonalMoves = 1;
                maxDiagonalMoves = int.MaxValue;
                break;
        }

        //Revisa y aplica movimientos respectivos a las habilidades de la pieza.
        //Debug.Log(piece.pieceName + ": piece.abilityGroup: " + piece.abilityGroup);
        /*
        if (piece.abilityGroup >= 0)
        {
            //Debug.Log("Estoy entrando al if");

            if (piece.GetAbilityGroup().CheckAbilities(piece, destination) == false)
            {
                Debug.Log("Ninguna habilidad permite este movimiento.");
                return false;
            }
        }*/

        #region General movement rule.

        if (!hasSpecialMovement)
        {
            if (minDiagonalMoves > 0)
            {
                if (move.x == move.y)
                {
                    if (move.x >= minDiagonalMoves)
                    {
                        if (move.x <= maxDiagonalMoves)
                        {
                            return CheckDiagonalPath(piece, destination);
                        }
                    }
                }
            }

            if (minHorizontalMoves > 0 || minVerticalMoves > 0)
            {
                if (move.x > 0 && move.y > 0)
                {
                }
                else
                {
                    if (move.x >= minHorizontalMoves && move.x > 0)
                    {
                        if (move.x <= maxHorizontalMoves)
                        {
                            return CheckPath(piece, 0, piece.position.x, destination.x);
                        }
                    }
                    if (move.y >= minVerticalMoves && move.y > 0)
                    {
                        if (move.y <= maxVerticalMoves)
                        {
                            return CheckPath(piece, 1, piece.position.y, destination.y);
                        }
                    }
                }
            }
        }
        #endregion

        return false;
    }
    private bool CheckPath(ChessPiece piece, int moveType, int initialPosition, int finalPosition)
    {
        //Returns true if there are no obstacles in the path of the piece.
        //movetype: 0 = horizontal, 1 = vertical
        int move = finalPosition - initialPosition;

        //Convierte los valores negativos en positivos para que no hayan movimientos en negativo.
        if (move < 0)
        {
            move = move * -1;
        }

        if (!canFly && move > 1)
        {
            //Si la posición inicial es mayor a la final, intercambia los valores para facilitar el recorrido.
            if (initialPosition > finalPosition)
            {
                int auxiliary = initialPosition;
                initialPosition = finalPosition;
                finalPosition = auxiliary;
            }

            for (int i = initialPosition + 1; i < finalPosition; i++)
            {
                if (moveType == 0)
                {
                    if (gridSystem.IsCellOccupied(new Vector2Int(i, piece.position.y)))
                    {
                        Debug.LogWarning("Celda ocupada detectada en la celda " + i + ", " + piece.position.y);
                        return false;
                    }
                }
                else
                {
                    if (gridSystem.IsCellOccupied(new Vector2Int(piece.position.x, i)))
                    {
                        Debug.LogWarning("Celda ocupada detectada en la celda " + piece.position.x + ", " + i);
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private bool CheckDiagonalPath(ChessPiece piece, Vector2Int destination)
    {
        //Returns true if there are no obstacles in the path of the piece.
        Vector2Int initialPosition = new Vector2Int(piece.position.x, piece.position.y);
        Vector2Int finalPosition = new Vector2Int(destination.x, destination.y);
        Vector2Int move = finalPosition - initialPosition;

        //Convierte los valores negativos en positivos para que no hayan movimientos en negativo.
        if (move.x < 0)
        {
            move.x = move.x * -1;
        }
        if (move.y < 0)
        {
            move.y = move.y * -1;
        }

        if (!canFly)
        {
            for (int i = 1; i < move.x; i++)
            {
                if (destination.x > piece.position.x)
                {
                    if (destination.y > piece.position.y)
                    {
                        if (gridSystem.IsCellOccupied(piece.position + new Vector2Int(i, i)))
                        {
                            Debug.LogWarning("Celda ocupada detectada en la celda " + i + ", " + i);
                            return false;
                        }
                    }
                    else
                    {
                        if (gridSystem.IsCellOccupied(piece.position + new Vector2Int(i, -i)))
                        {
                            Debug.LogWarning("Celda ocupada detectada en la celda " + i + ", " + -i);
                            return false;
                        }
                    }
                }
                else
                {
                    if (destination.y > piece.position.y)
                    {
                        Debug.Log("Revisando celda en: " + (piece.position.x - i) + ", " + (piece.position.y + i));
                        if (gridSystem.IsCellOccupied(piece.position + new Vector2Int(-i, i)))
                        {
                            Debug.LogWarning("Celda ocupada detectada en la celda " + -i + ", " + i);
                            return false;
                        }
                    }
                    else
                    {
                        if (gridSystem.IsCellOccupied(piece.position + new Vector2Int(-i, -i)))
                        {
                            Debug.LogWarning("Celda ocupada detectada en la celda " + -i + ", " + -i);
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public GridSystem GetGridSystem()
    {
        return this.gridSystem;
    }

    public int GetHorizontalPosition()
    {
        return position.x;
    }

    public int GetVerticalPosition()
    {
        return position.y;
    }

    public void MovePiece(Vector2Int destination, ChessPiece capturedPieceIfExists)
    {
        Vector2Int from = gridSystem.GetGridPosition(this.transform.position);

        gridSystem.SetCellAsOccupied(this.position, false);
        this.SetPosition(destination);
        this.hasMoved = true;
        gridSystem.SetCellAsOccupied(destination, true);

        gridSystem.RegisterMove(this.position, destination, this.team);

        Debug.Log($"Se movio la pieza a " + destination);

        inputManager.highlightManager.ClearMoveHighlights();
        inputManager.highlightManager.ShowLastMove(Teams.TeamName.White);

        inputManager.RegisterMoveInHistory(this, gridSystem.GetGridPosition(this.transform.position), destination, capturedPieceIfExists);

        inputManager.SelectPiece(this, false);
    }
}
