using UnityEngine;

public abstract class AIState
{
    protected ChessAI ai;

    public AIState(ChessAI ai)
    {
        this.ai = ai;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
