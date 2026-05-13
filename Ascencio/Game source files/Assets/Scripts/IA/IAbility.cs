public interface IAbility
{
    int Evaluate(BoardState board, AIMove move);
    void Execute(GridSystem grid, AIMove move);
}
