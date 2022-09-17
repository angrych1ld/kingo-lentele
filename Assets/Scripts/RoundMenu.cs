using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMenu : MonoBehaviour
{
    public RoundSlider[] sliders = new RoundSlider[4];

    private int rowIndex;
    private int cellIndex;

    public void Show(GameState gameState, int rowIndex, int cellIndex, GameConfig.RoundConfig config)
    {
        RoundState state = gameState.players[rowIndex].rounds[cellIndex];
        this.rowIndex = rowIndex;
        this.cellIndex = cellIndex;

        for (int i = state.points.Length - 1; i < sliders.Length; i++)
        {
            sliders[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < state.points.Length; i++)
        {
            sliders[i].slider.maxValue = config.tickCount;
            sliders[i].slider.SetValueWithoutNotify(state.points[i] / config.tickValue);
            sliders[i].text.text = gameState.players[i].playerName;
        }
    }

    public void OnCloseClick()
    {
        Overlord.instance.OnRoundMenuCloseClick();
    }

    public void OnApplyClick()
    {
        Overlord.instance.OnRoundMenuApplyClick(rowIndex, cellIndex);
    }
}
