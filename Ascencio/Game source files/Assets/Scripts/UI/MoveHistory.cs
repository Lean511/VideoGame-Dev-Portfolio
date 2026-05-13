using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MoveHistory : MonoBehaviour
{
    public TextMeshProUGUI historyText;

    private List<string> moveTexts = new();
    private List<MoveData> moveData = new();

    public void AddMove(MoveData data)
    {
        moveData.Add(data);

        string text = BuildMoveText(data);
        moveTexts.Add(text);

        UpdateUI();
    }

    public List<MoveData> GetMovesForPiece(ChessPiece piece)
    {
        return moveData.FindAll(m => m.piece == piece);
    }

    private void UpdateUI()
    {
        if (historyText != null)
            historyText.text = string.Join("\n", moveTexts);
    }

    public void ClearHistory()
    {
        moveTexts.Clear();
        moveData.Clear();
        UpdateUI();
    }

    public string BuildMoveText(MoveData move)
    {
        string text = $"Turno {move.turn + 1}: {move.team} movió {move.piece.pieceName} a {move.to}";

        if (move.capturedPiece != null)
        {
            text += $" y comió {move.capturedPiece.pieceName}";
        }

        return text;
    }
}
