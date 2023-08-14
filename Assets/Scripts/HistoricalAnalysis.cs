using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HistoricalAnalysis
{
    public List<Game> games;
    public List<StatAnalysis> statAnalyses = new List<StatAnalysis>();

    public static HistoricalAnalysis Create(IEnumerable<GameState> states)
    {
        HistoricalAnalysis analysis = new HistoricalAnalysis
        {
            games = states
            .Select(s => new Game
            {
                gameState = s,
                completion = s.GetCompletion(),
                scores = s.CalculateScores()
            })
            .Where(g => g.completion >= 0.75f)
            .ToList()
        };

        AddTotalGames(analysis);
        AddAveragePoints(analysis);

        return analysis;
    }

    private static void AddTotalGames(HistoricalAnalysis history)
    {
        Dictionary<string, PlayerStatValue> nameToPlayer = new Dictionary<string, PlayerStatValue>();
        foreach (Game game in history.games)
        {
            foreach (var p in game.gameState.players)
            {
                string k = p.playerName.Trim().ToLower();
                if (!nameToPlayer.TryGetValue(k, out var psv))
                {
                    psv = new PlayerStatValue { playerName = p.playerName };
                    nameToPlayer.Add(k, psv);
                }
                psv.statCount++;
                psv.statValue++;
            }
        }
        history.statAnalyses.Add(new StatAnalysis
        {
            statTitle = "Viso sužaista žaidimų",
            players = nameToPlayer.Values.ToList()
        });
    }

    private static void AddAveragePoints(HistoricalAnalysis history)
    {
        Dictionary<string, PlayerStatValue> nameToPlayer = new Dictionary<string, PlayerStatValue>();
        foreach (Game game in history.games)
        {
            for (int i = 0; i < game.gameState.players.Length; i++)
            {
                string k = game.gameState.players[i].playerName.Trim().ToLower();
                if (!nameToPlayer.TryGetValue(k, out var psv))
                {
                    psv = new PlayerStatValue { playerName = game.gameState.players[i].playerName };
                    nameToPlayer.Add(k, psv);
                }
                psv.statCount++;

                psv.statValue += game.scores[i];
            }
        }

        foreach (var p in nameToPlayer.Values)
        {
            p.statValue /= p.statCount;
        }

        history.statAnalyses.Add(new StatAnalysis
        {
            statTitle = "Vidutinis taškų kiekis per žaidimą",
            players = nameToPlayer.Values.ToList()
        });
    }

    public class Game
    {
        public float completion;
        public GameState gameState;
        public int[] scores;
    }

    public class StatAnalysis
    {
        public string statTitle;
        public List<PlayerStatValue> players = new List<PlayerStatValue>();

        public float GetValueRange()
        {
            if (players.Count == 0)
            {
                return 0;
            }

            return players.Max(p => p.statValue) - players.Min(p => p.statValue);
        }
    }

    public class PlayerStatValue
    {
        public string playerName;
        public float statValue;
        public float statCount;
    }
}
