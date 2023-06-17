using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RoundMenu : MonoBehaviour
{
    public RoundSlider[] sliders = new RoundSlider[4];
    [SerializeField] private TextMeshProUGUI titleText = null;
    [SerializeField] private Button applyButton = null;

    private int rowIndex;
    private int cellIndex;
    private GameConfig.RoundConfig roundConfig;

    private void Awake()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            RoundSlider rs = sliders[i];
            int index = i;
            rs.slider.onValueChanged.AddListener(OnSliderChanged);

            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            entry.callback.AddListener((e) => OnSliderPointerClick(e, index));
            rs.eventTrigger.triggers.Add(entry);
        }
    }

    public void Show(GameState gameState, int rowIndex, int cellIndex, GameConfig.RoundConfig config)
    {
        roundConfig = config;
        titleText.text = config.title;

        RoundState state = gameState.players[rowIndex].rounds[cellIndex];
        this.rowIndex = rowIndex;
        this.cellIndex = cellIndex;

        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].gameObject.SetActive(i < state.points.Length);
        }

        for (int i = 0; i < state.points.Length; i++)
        {
            sliders[i].slider.maxValue = config.tickCount;
            sliders[i].slider.SetValueWithoutNotify(state.points[i] / config.tickValue);
            sliders[i].playerName = gameState.players[i].playerName;
            sliders[i].SetStepCount(config.tickCount);
        }

        UpdateMenuState();
    }

    private void OnSliderPointerClick(BaseEventData baseData, int sliderIndex)
    {
        PointerEventData data = baseData as PointerEventData;

        if (data.button == PointerEventData.InputButton.Right)
        {
            int totalTicks = 0;
            for (int i = 0; i < sliders.Length; i++)
            {
                if (i == sliderIndex) continue;
                totalTicks += (int)sliders[i].slider.value;
            }
            sliders[sliderIndex].slider.value = Mathf.Clamp(roundConfig.tickCount - totalTicks, 0, roundConfig.tickCount);
        }
    }

    private void OnSliderChanged(float v)
    {
        UpdateMenuState();
    }

    private void UpdateMenuState()
    {
        int totalTicks = 0;
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].text.text = sliders[i].playerName + System.Environment.NewLine +
                sliders[i].slider.value * roundConfig.tickValue +
                "(" + sliders[i].slider.value + ")";
            totalTicks += (int)sliders[i].slider.value;
        }
        applyButton.interactable = totalTicks == roundConfig.tickCount || totalTicks == 0;
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
