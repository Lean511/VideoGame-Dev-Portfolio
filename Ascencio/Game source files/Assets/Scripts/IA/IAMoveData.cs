using UnityEngine;

public class AIMoveData
{
    public Vector2Int from;
    public Vector2Int to;

    public AIMoveData(Vector2Int from, Vector2Int to)
    {
        this.from = from;
        this.to = to;
    }
}