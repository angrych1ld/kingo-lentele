using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableCell : MonoBehaviour
{
    [SerializeField] private Text symbolText = null;
    [SerializeField] private Button cellButton = null;
    [SerializeField] private Image bgImage = null;
    [SerializeField] private GameObject scoresContainer = null;
    [SerializeField] private Text[] scores = new Text[0];

    public string[] symbols = new string[] { "K", "B", "D", "C", "KIR", "2P", "E-", "E+", "1A", "2A" };

    private int rowIndex;
    private int cellIndex;

    public void Initialize(string symbol, int rowIndex, int cellIndex)
    {
        symbolText.text = symbol;
        this.rowIndex = rowIndex;
        this.cellIndex = cellIndex;
    }

    public void ApplyState(GameState gameState, int row, int col)
    {
        RoundState state = gameState.players[row].rounds[col];
        bool played = false;
        for (int i = 0; i < state.points.Length; i++)
        {
            if (state.PointsAssigned())
            {
                played = true;
            }
        }

        if (played)
        {
            bgImage.color = Color.gray;
            symbolText.gameObject.SetActive(false);
            scoresContainer.gameObject.SetActive(true);

            for (int i = 0; i < scores.Length; i++)
            {
                if (i < state.points.Length)
                {
                    scores[i].transform.parent.gameObject.SetActive(true);
                    scores[i].text = gameState.players[i].playerName[0] + ": " + state.points[i];
                }
                else
                {
                    scores[i].transform.parent.gameObject.SetActive(false);
                }

            }
        }
        else
        {
            bgImage.color = Color.white;
            symbolText.gameObject.SetActive(true);
            scoresContainer.gameObject.SetActive(false);
        }
    }

    public void OnCellClick()
    {
        Overlord.instance.OnCellClick(rowIndex, cellIndex);
    }
}
