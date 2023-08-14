using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    public Image bar;
    public TextMeshProUGUI label;

    public float maxBarWidth = 900;
    public float minBarWidth = 20;

    public void SetBarSize01(float t)
    {
        bar.rectTransform.sizeDelta = new Vector2(
            Mathf.Clamp(maxBarWidth * t, minBarWidth, maxBarWidth),
            bar.rectTransform.sizeDelta.y);
    }
}
