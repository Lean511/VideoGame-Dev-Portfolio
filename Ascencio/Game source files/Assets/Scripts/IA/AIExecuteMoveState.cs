using System.Collections;
using UnityEngine;

public class AIExecuteMoveState : AIState
{
    public AIExecuteMoveState(ChessAI ai) : base(ai) { }

    public override void Enter()
    {
        Debug.Log("IA ejecutando movimiento");
        ai.StartCoroutine(ExecuteWithDelay());
    }

    private IEnumerator ExecuteWithDelay()
    {
        yield return new WaitForSeconds(1f);

        ExecuteMove();
        ai.ChangeState(ai.idleState);
    }

    private void ExecuteMove()
    {
        Debug.Log("IAExecuteMoveState: ExecuteMove llamado");

        if (!ai.hasMove)
        {
            return;
        }

        AIMove move = ai.chosenMove;

        ChessPiece capturedPiece = null;

        if (ai.grid.IsCellOccupied(move.to))
        {
            ChessPiece target = ai.grid.GetPieceOnGridPosition(move.to);
            if (target.team != move.piece.team)
            {
                capturedPiece = target;
                target.Eat();

                if (GameManager.Instance != null)
                    GameManager.Instance.OnPieceCaptured(target);
            }
        }

        ai.grid.SetCellAsOccupied(move.from, false);
        move.piece.SetPosition(move.to);
        move.piece.hasMoved = true;
        ai.grid.SetCellAsOccupied(move.to, true);

        ai.grid.RegisterMove(move.from, move.to, ai.aiTeam);

        MoveData moveData = new MoveData
        {
            piece = move.piece,
            from = move.from,
            to = move.to,
            turn = ai.grid.turn,
            capturedPiece = capturedPiece,
            team = ai.aiTeam
        };

        ai.history.AddMove(moveData);

        HighlightManager[] highlightManager = GameObject.FindObjectsByType<HighlightManager>(FindObjectsSortMode.None);
        foreach (var manager in highlightManager)
        {
            manager.ShowLastMove(Teams.TeamName.White);
        }

        // ¿Rey blanco en jaque?
        if (ai.grid.checkService.IsKingInCheck(Teams.TeamName.White))
        {
            Debug.Log("Rey BLANCO en jaque");

            // ¿Jaque mate?
            if (ai.grid.checkmateService.IsCheckmate(Teams.TeamName.White))
            {
                Debug.Log("JAQUE MATE — GANAN LAS NEGRAS");

                if (GameManager.Instance != null)
                    GameManager.Instance.EndGame(Teams.TeamName.White);

                return;
            }
        }

        ai.grid.turn++;
        ai.hasMove = false;

        InputManager input = Object.FindFirstObjectByType<InputManager>();
        input.currentTurnTeam = Teams.TeamName.White;
    }
}
