using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableCell : MonoBehaviour
{
    [SerializeField] private Text symbolText = null;
    [SerializeField] private Button cellButton = null;
    [SerializeField] private Image bgImage = null;

    public string[] symbols = new string[] { "K", "B", "D", "C", "KIR", "2P", "E-", "E+", "1A", "2A" };

    private int rowIndex;
    private int cellIndex;

    public void Initialize(string symbol, int rowIndex, int cellIndex)
    {
        symbolText.text = symbol;
        this.rowIndex = rowIndex;
        this.cellIndex = cellIndex;
    }

    public void ApplyState(RoundState state)
    {
        bool played = false;
        for (int i = 0; i < state.points.Length; i++)
        {
            if (state.PointsAssigned())
            {
                played = true;
            }
        }

        bgImage.color = played ? Color.gray : Color.white;
    }

    public void OnCellClick()
    {
        Overlord.instance.OnCellClick(rowIndex, cellIndex);
    }
}
