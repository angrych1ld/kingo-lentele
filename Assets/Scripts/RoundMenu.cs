using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundMenu : MonoBehaviour
{
    public RoundSlider[] sliders = new RoundSlider[4];
    [SerializeField] private Text titleText = null;

    private int rowIndex;
    private int cellIndex;
    private GameConfig.RoundConfig roundConfig;

    private void Awake()
    {
        foreach (RoundSlider rs in sliders)
        {
            rs.slider.onValueChanged.AddListener(OnSliderChanged);
        }
    }

    public void Show(GameState gameState, int rowIndex, int cellIndex, GameConfig.RoundConfig config)
    {
        roundConfig = config;
        titleText.text = config.title;

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
            sliders[i].playerName = gameState.players[i].playerName;
        }

        UpdateSliderLabels();
    }

    private void OnSliderChanged(float v)
    {
        UpdateSliderLabels();
    }

    private void UpdateSliderLabels()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].text.text = sliders[i].playerName + " " +
                sliders[i].slider.value * roundConfig.tickValue +
                "(" + sliders[i].slider.value + ")";
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
