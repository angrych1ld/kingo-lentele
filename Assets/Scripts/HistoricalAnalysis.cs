using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HistoricalAnalysis
{
    public List<Game> games;
    public List<StatAnalysis> statAnalyses = new List<StatAnalysis>();
    public GameConfig config3p;

    public static HistoricalAnalysis Create(IEnumerable<GameState> states, GameState currentState, GameConfig config3p)
    {
        HistoricalAnalysis analysis = new HistoricalAnalysis
        {
            config3p = config3p,
            games = new List<Game>()
        };

        if (currentState.GetCompletion() >= 1f)
        {
            analysis.games.Add(new Game
            {
                gameState = currentState,
                completion = currentState.GetCompletion(),
                scores = currentState.CalculateScores()
            });
        }

        analysis.games.AddRange(states.Select(s => new Game
        {
            gameState = s,
            completion = s.GetCompletion(),
            scores = s.CalculateScores()
        })
            .Where(g => g.completion >= 0.75f));

        //AddTotalGames(analysis);
        AddAveragePoints(analysis);
        AddAveragePointsInGameCategories(analysis);

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

    private static void AddAveragePointsInGameCategories(HistoricalAnalysis analysis)
    {
        for (int g = 0; g < 10; g++)
        {
            Dictionary<string, PlayerStatValue[]> nameToPlayer = new Dictionary<string, PlayerStatValue[]>();

            foreach (Game game in analysis.games)
            {
                for (int i = 0; i < game.gameState.players.Length; i++)
                {
                    string k = game.gameState.players[i].playerName.Trim().ToLower();
                    if (!nameToPlayer.TryGetValue(k, out var psv))
                    {
                        psv = new PlayerStatValue[] {
                            new PlayerStatValue{ playerName = game.gameState.players[i].playerName },
                            new PlayerStatValue{ playerName = game.gameState.players[i].playerName }
                        };
                        nameToPlayer.Add(k, psv);
                    }
                    psv[0].statCount++;
                    psv[1].statCount++;

                    for (int j = 0; j < game.gameState.players.Length; j++)
                    {
                        psv[0].statValue += game.gameState.players[j].rounds[g].points[i];
                    }
                    psv[1].statValue += game.gameState.players[i].rounds[g].points[i];
                }
            }

            foreach (var p in nameToPlayer.Values)
            {
                p[0].statValue /= p[0].statCount;
                p[1].statValue /= p[1].statCount;
            }

            analysis.statAnalyses.Add(new StatAnalysis
            {
                statTitle = "Vidutinis taškų kiekis per žaidimus: " +
                    analysis.config3p.rounds[g].title,
                players = nameToPlayer.Values.Select(v => v[0]).ToList()
            });

            analysis.statAnalyses.Add(new StatAnalysis
            {
                statTitle = "Vidutinis taškų kiekis per <b>savo</b> žaidimus: " +
                    analysis.config3p.rounds[g].title,
                players = nameToPlayer.Values.Select(v => v[1]).ToList()
            });
        }
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
            statTitle = "Vidutinis taškų kiekis per visą žaidimą",
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
