using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    public Text existingGameText;
    public Button p3Button;
    public Button p4Button;
    public InputField[] playerNameInputs = new InputField[4];

    private void OnEnable()
    {
        GameState existing = SaveManager.LoadCurrentGame();
        existingGameText.text =
            Mathf.FloorToInt(existing.GetCompletion() * 100) +
            "% baigta - " +
            string.Join(", ", existing.players.Select(p => p.playerName));
        OnPlayerNameInputChanged();
    }

    public void OnContinueClick()
    {
        Overlord.instance.StartGame(SaveManager.LoadCurrentGame());
    }

    public void OnNew3PlayersClick()
    {
        SaveManager.SaveCurrentGame(GameState.New(
            playerNameInputs.Take(3).Select(i => i.text).ToArray()));
        Overlord.instance.StartGame(SaveManager.LoadCurrentGame());
    }

    public void OnNew4PlayersClick()
    {
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
