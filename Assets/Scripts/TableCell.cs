using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI centerText = null;
    [SerializeField] private TextMeshProUGUI topLeftText;
    [SerializeField] private TextMeshProUGUI botRightText;

    [SerializeField] private Button cellButton = null;
    [SerializeField] private Image bgImage = null;
    [SerializeField] private GameObject scoresContainer = null;
    [SerializeField] private TextMeshProUGUI[] scores = new TextMeshProUGUI[0];

    [SerializeField] private Sprite emptyCardSprite;
    [SerializeField] private Sprite cardBgSprite;

    public static string[] symbols = new string[] { "K", "B", "D", "♥", "K", "2", "E-", "E+", "1A", "2A" };
    public static string[] titles = new string[] {
        "Kingas",
        "Bartukai",
        "Damos",
        "Čirvai",
        "Kirčiai",
        "Du Paskutiniai",
        "Minusinis Eralašas",
        "Pliusinis Eralašas",
        "Pirmasis Atsilošimas",
        "Antrasis Atsilošimas"
    };

    private int rowIndex;
    private int cellIndex;

    public void Initialize(string title, string symbol, int rowIndex, int cellIndex)
    {
        centerText.text = title;
        topLeftText.text = symbol;
        botRightText.text = symbol;
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
            bgImage.sprite = cardBgSprite;
            centerText.gameObject.SetActive(false);
            topLeftText.gameObject.SetActive(false);
            botRightText.gameObject.SetActive(false);
            scoresContainer.gameObject.SetActive(true);

            for (int i = 0; i < scores.Length; i++)
            {
                if (i < state.points.Length)
                {
                    scores[i].transform.parent.gameObject.SetActive(true);
                    scores[i].text = state.points[i] + " (" + gameState.players[i].playerName[0] + ")";
                }
                else
                {
                    scores[i].transform.parent.gameObject.SetActive(false);
                }

            }
        }
        else
        {
            bgImage.sprite = emptyCardSprite;

            centerText.gameObject.SetActive(true);
            topLeftText.gameObject.SetActive(true);
            botRightText.gameObject.SetActive(true);

            scoresContainer.gameObject.SetActive(false);
        }
    }

    public void OnCellClick()
    {
        Overlord.instance.OnCellClick(rowIndex, cellIndex);
    }
}
