using System.Collections.Generic;
using UnityEngine;

public class AIThinkState : AIState
{
    public AIThinkState(ChessAI ai) : base(ai) { }

    public override void Enter()
    {
        DecideMove();
        ai.ChangeState(ai.executeMoveState);
    }

    private void DecideMove()
    {
        List<AIMove> possibleMoves = new List<AIMove>();

        Teams.TeamName enemyTeam =
        ai.aiTeam == Teams.TeamName.White
        ? Teams.TeamName.Black
        : Teams.TeamName.White;

        foreach (ChessPiece piece in ai.grid.GetAllPieces())
        {
            if (piece.team != ai.aiTeam)
                continue;

            for (int x = 0; x < ai.grid.width; x++)
            {
                for (int y = 0; y < ai.grid.height; y++)
                {
                    Vector2Int target = new Vector2Int(x, y);

                    if (!piece.isMovementLegal(piece, target))
                        continue;

                    if (ai.grid.IsCellOccupied(target))
                    {
                        ChessPiece other = ai.grid.GetPieceOnGridPosition(target);
                        if (other.team == piece.team)
                            continue;
                    }

                    int score = 0;
                    //Comer
                    if (ai.grid.IsCellOccupied(target))
                    {
                        ChessPiece targetPiece = ai.grid.GetPieceOnGridPosition(target);
                        if (targetPiece.team != piece.team)
                        {
                            // Si es el rey, tiene mayor prioridad
                            if (targetPiece.pieceType == PieceType.Type.King)
                            {
                                score += 100000; // básicamente "ganar"
                            }
                            else
                            {
                                score += PieceValues.GetValue(targetPiece.pieceType);
                            }
                        }
                    }


                    // Incentivar peones
                    if (piece.pieceType == PieceType.Type.Pawn)
                    {
                        score += 8;

                        if (!piece.hasMoved)
                            score += 4;
                    }

                    //Bonus por avanzar
                    if (piece.team == Teams.TeamName.Black)
                    {
                        score += Mathf.RoundToInt(target.y * 0.5f);
                    }
                    else
                    {
                        score += Mathf.RoundToInt((7 - target.y) * 0.5f);
                    }

                    // Penalizar piezas que ya se han movido
                    int movesWithThisPiece = ai.history.GetMovesForPiece(piece).Count;
                    if (movesWithThisPiece > 0)
                    {
                        score -= 6;
                    }

                    // Incentivar desarrollo temprano
                    if (ai.grid.turn < 6)
                    {
                        if (piece.pieceType == PieceType.Type.Pawn ||
                            piece.pieceType == PieceType.Type.Knight ||
                            piece.pieceType == PieceType.Type.Bishop)
                        {
                            score += 5;
                        }
                    }

                    // Penalizar regalar piezas
                    int myValue = PieceValues.GetValue(piece.pieceType);
                    if (ai.grid.checkService.WouldLeaveKingInCheck(
                            piece,
                            piece.position,
                            target))
                    {
                        score -= myValue * 2;
                    }

                    // Simular el movimiento para ver si da jaque
                    if (ai.grid.checkService.WouldMoveGiveCheck(
                            piece,
                            piece.position,
                            target,
                            Teams.TeamName.White)) // el enemigo
                    {
                        score += 5000; // dar jaque es MUY bueno
                    }

                    // Simular jaque mate
                    if (ai.grid.checkmateService.WouldMoveBeCheckmate(
                            piece,
                            piece.position,
                            target,
                            Teams.TeamName.White))
                    {
                        score += 1_000_000; // nada supera esto
                    }

                    possibleMoves.Add(new AIMove
                    {
                        piece = piece,
                        from = piece.position,
                        to = target,
                        score = score
                    });
                }
            }
        }



        possibleMoves.RemoveAll(move =>
            ai.grid.checkService.WouldLeaveKingInCheck(
                move.piece,
                move.from,
                move.to
            )
        );

        if (possibleMoves.Count == 0)
        {
            Debug.Log("IA: no hay movimientos legales");
            ai.hasMove = false;
            return;
        }

        possibleMoves.Sort((a, b) => b.score.CompareTo(a.score));
        ai.chosenMove = possibleMoves[0];
        ai.hasMove = true;

        if (possibleMoves.Count == 0)
            return;


        foreach (var m in possibleMoves)
        {
        }

        if (possibleMoves.Count == 0)
        {
            Debug.Log("IA: no hay movimientos legales (rey atrapado)");
            ai.hasMove = false;
            return;
        }

        possibleMoves.Sort((a, b) => b.score.CompareTo(a.score));
        ai.chosenMove = possibleMoves[0];
        ai.hasMove = true;
    }
}
