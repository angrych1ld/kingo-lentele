using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableRow : MonoBehaviour
{
    public Text nameText;

    [SerializeField] private TableCell cellPrefab = null;
    [SerializeField] private Transform cellParent = null;
    private List<TableCell> cells = new List<TableCell>();

    public void Initialize(int cellCount, int rowIndex)
    {
        foreach (TableCell c in cellParent.GetComponentsInChildren<TableCell>())
        {
            Destroy(c.gameObject);
        }
        cells.Clear();

        for (int i = 0; i < cellCount; i++)
        {
            TableCell c = Instantiate(cellPrefab, cellParent);
            c.Initialize(c.symbols[i], rowIndex, i);
            cells.Add(c);
        }
    }

    public void ApplyState(PlayerState state)
    {
        if (cells.Count != state.rounds.Length)
        {
            foreach (TableCell c in cellParent.GetComponentsInChildren<TableCell>())
            {
                Destroy(c.gameObject);
            }
            cells.Clear();

            for (int i = 0; i < state.rounds.Length; i++)
            {
                TableCell c = Instantiate(cellPrefab, cellParent);
                cells.Add(c);
            }
        }

        for (int i = 0; i < state.rounds.Length && i < cells.Count; i++)
        {
            cells[i].ApplyState(state.rounds[i]);
        }
        nameText.text = state.playerName;
    }
}
