using UnityEngine;

public class AIIdleState : AIState
{
    public AIIdleState(ChessAI ai) : base(ai) { }

    public override void Enter()
    {
        // No hace nada, espera su turno
    }
}
