public static class PieceValues
{
    public static int GetValue(PieceType.Type type)
    {
        return type switch
        {
            PieceType.Type.Pawn => 10,
            PieceType.Type.Knight => 30,
            PieceType.Type.Bishop => 30,
            PieceType.Type.Rook => 50,
            PieceType.Type.Queen => 90,
            PieceType.Type.King => 900,
            _ => 0
        };
    }
}
