using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI existingGameText;
    public Button p3Button;
    public Button p4Button;
    public TMP_InputField[] playerNameInputs = new TMP_InputField[4];

    public GameObject historicalDataParent;
    public Transform pastGamesParent;
    public PastGamePlayerEntry pastGameEntryPrefab;
    public List<PastGamePlayerEntry> pastGameEntries = new List<PastGamePlayerEntry>();

    private HistoricalAnalysis historicalAnalysis;

    private void Awake()
    {
        foreach (var input in playerNameInputs)
        {
            input.onValueChanged.AddListener((s) => OnPlayerNameInputChanged());
        }
    }

    private void OnEnable()
    {
        GameState existing = SaveManager.LoadCurrentGame();
        int[] scores = existing.CalculateScores();

        existingGameText.text =
            Mathf.FloorToInt(existing.GetCompletion() * 100) +
            "% baigta" + System.Environment.NewLine;
        for (int i = 0; i < scores.Length; i++)
        {
            existingGameText.text += "<nobr>" + existing.players[i].playerName + "</nobr>" +
                " - " + scores[i] + System.Environment.NewLine;
        }



        OnPlayerNameInputChanged();

        byte[] historicalData = SaveManager.GetHistoricalData();
        List<GameState> historicalGames = GameState.DeserializeArray(historicalData);
        historicalAnalysis = HistoricalAnalysis.Create(historicalGames);

        if (historicalAnalysis.games.Count == 0)
        {
            historicalDataParent.gameObject.SetActive(false);
        }
        else
        {
            historicalDataParent.gameObject.SetActive(true);
            UpdatePastGames(historicalAnalysis);
        }
    }

    private void UpdatePastGames(HistoricalAnalysis historicalAnalysis)
    {
        foreach (PastGamePlayerEntry entry in pastGameEntries)
        {
            Destroy(entry.gameObject);
        }
        pastGameEntries.Clear();

        for (int i = 0; i < historicalAnalysis.games.Count && i < 10; i++)
        {
            HistoricalAnalysis.Game game = historicalAnalysis.games[i];
            for (int j = 0; j < 4; j++)
            {
                PastGamePlayerEntry entry = Instantiate(pastGameEntryPrefab, pastGamesParent);
                pastGameEntries.Add(entry);
                int maxScore = Mathf.Max(game.scores);
                entry.text.text = j < game.scores.Length
                    ? "<nobr>" + game.gameState.players[j].playerName + "</nobr>" + ": " + game.scores[j]
                    : "";
                if (j < game.scores.Length && game.scores[j] == maxScore)
                {
                    entry.text.fontStyle = TMPro.FontStyles.Underline;
                }
            }
        }
    }

    public void OnContinueClick()
    {
        Overlord.instance.StartGame(SaveManager.LoadCurrentGame());
    }

    public void OnNew3PlayersClick()
    {
        SaveManager.AppendHistoricalGame(SaveManager.LoadCurrentGame());
        SaveManager.SaveCurrentGame(GameState.New(
            playerNameInputs.Take(3).Select(i => i.text).ToArray()));
        Overlord.instance.StartGame(SaveManager.LoadCurrentGame());
    }

    public void OnNew4PlayersClick()
    {
        SaveManager.AppendHistoricalGame(SaveManager.LoadCurrentGame());
        SaveManager.SaveCurrentGame(GameState.New(
            playerNameInputs.Take(4).Select(i => i.text).ToArray()));
        Overlord.instance.StartGame(SaveManager.LoadCurrentGame());
    }

    public void OnPlayerNameInputChanged()
    {
        p3Button.interactable = playerNameInputs.Take(3).All(i => !string.IsNullOrWhiteSpace(i.text));
        p4Button.interactable = playerNameInputs.Take(4).All(i => !string.IsNullOrWhiteSpace(i.text));
    }
}
