﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlord : MonoBehaviour
{
    public static Overlord instance;

    public GameConfig[] configs = new GameConfig[4];

    [SerializeField] private MainMenu menu;
    [SerializeField] private Table table;
    [SerializeField] private RoundMenu roundMenu;


    private GameState currentState;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        menu.gameObject.SetActive(true);
    }

    public void StartGame(GameState state)
    {
        currentState = state;
        table.ApplyGameState(currentState, configs[currentState.players.Length]);
        menu.gameObject.SetActive(false);
        roundMenu.gameObject.SetActive(false);
    }

    public void OnCellClick(int rowIndex, int cellIndex)
    {
        if (menu.gameObject.activeSelf)
        {
            return;
        }

        roundMenu.Show(
            currentState,
            rowIndex,
            cellIndex,
            configs[currentState.players.Length].rounds[cellIndex]);
        roundMenu.gameObject.SetActive(true);
    }

    public void OnMenuButtonClick()
    {
        menu.gameObject.SetActive(true);
        roundMenu.gameObject.SetActive(false);
    }

    public void OnRoundMenuCloseClick()
    {
        roundMenu.gameObject.SetActive(false);
    }

    public void OnRoundMenuApplyClick(int rowIndex, int cellIndex)
    {
        GameConfig config = configs[currentState.players.Length];

        for (int i = 0; i < currentState.players.Length; i++)
        {
            currentState.players[rowIndex].rounds[cellIndex].points[i]
                = Mathf.RoundToInt(roundMenu.sliders[i].slider.value) *
                config.rounds[cellIndex].tickValue;
        }

        SaveManager.SaveCurrentGame(currentState);
        table.ApplyGameState(currentState, config);
        roundMenu.gameObject.SetActive(false);
    }
}
