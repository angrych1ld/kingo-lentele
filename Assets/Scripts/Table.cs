using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private TableRow rowPrefab = null;
    [SerializeField] private Transform rowParent = null;
    private List<TableRow> rows = new List<TableRow>();

    public void ApplyGameState(GameState state, GameConfig config)
    {
        if (rows.Count != state.players.Length)
        {
            foreach (TableRow r in rowParent.GetComponentsInChildren<TableRow>())
            {
                Destroy(r.gameObject);
            }
            rows.Clear();

            for (int i = 0; i < state.players.Length; i++)
            {
                TableRow r = Instantiate(rowPrefab, rowParent);
                r.Initialize(GameState.ROUNDS_PER_PLAYER, i);
                rows.Add(r);
            }
        }

        int[] totalScores = CalculateScores(state);
        for (int i = 0; i < state.players.Length && i < rows.Count; i++)
        {
            rows[i].ApplyState(state.players[i], totalScores[i]);
        }
    }

    private int[] CalculateScores(GameState state)
    {
        int[] scores = new int[state.players.Length];
        for (int i = 0; i < state.players.Length; i++)
        {
            for (int j = 0; j < state.players[i].rounds.Length; j++)
            {
                for (int k = 0; k < state.players[i].rounds[j].points.Length; k++)
                {
                    scores[k] += state.players[i].rounds[j].points[k];
                }
            }
        }
        return scores;
    }
}
