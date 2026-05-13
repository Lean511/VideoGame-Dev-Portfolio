using Unity.VisualScripting;
using UnityEngine;

public class ChessAI : MonoBehaviour
{
    public Teams.TeamName aiTeam = Teams.TeamName.Black;
    public GridSystem grid;
    public MoveHistory history;

    private AIState currentState;

    // Estados
    public AIIdleState idleState;
    public AIThinkState thinkState;
    public AIExecuteMoveState executeMoveState;

    // Datos de decisión
    public bool hasMove;
    public AIMove chosenMove;

    void Awake()
    {
        idleState = new AIIdleState(this);
        thinkState = new AIThinkState(this);
        executeMoveState = new AIExecuteMoveState(this);

        ChangeState(idleState);
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(AIState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // Llamado desde GridSystem cuando toca el turno de la IA
    public void StartTurn()
    {
        Debug.Log(">>> IA: StartTurn llamado");
        //hasMove = false;
        ChangeState(thinkState);

        BoardState state = BoardStateBuilder.FromGrid(grid);
        var moves = MoveGenerator.GenerateMoves(state, aiTeam);
        foreach (var m in moves)
        {
            Debug.Log($"SIM MOVE: {m.from} -> {m.to}");
        }
        Debug.Log(state.board[4, 0]?.type); // debería loguear King
    }
}
