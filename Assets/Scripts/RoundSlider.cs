using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RoundSlider : MonoBehaviour
{
    public string playerName;
    public TextMeshProUGUI text;
    public Slider slider;
    public EventTrigger eventTrigger;
    public GameObject markerPrefab;
    public Transform markerParent;

    private GameObject[] markers = new GameObject[300];
    private int activeMarkers = 0;

    private void Awake()
    {
        for (int i = 0; i < markers.Length; i++)
        {
            markers[i] = Instantiate(markerPrefab, markerParent);
            markers[i].gameObject.SetActive(false);
        }
        activeMarkers = 0;
    }

    public void SetStepCount(int steps)
    {
        int targetMarkerCount = steps;
        if (steps > 50)
        {
            targetMarkerCount = 1;
        }

        for (int i = activeMarkers; i < targetMarkerCount; i++)
        {
            markers[i].gameObject.SetActive(true);
        }

        for (int i = targetMarkerCount; i < activeMarkers; i++)
        {
            markers[i].gameObject.SetActive(false);
        }

        activeMarkers = targetMarkerCount;
    }
}
