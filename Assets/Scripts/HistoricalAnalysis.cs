using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HistoricalAnalysis
{
    public List<Game> games;

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

        return analysis;
    }

    public class Game
    {
        public float completion;
        public GameState gameState;
        public int[] scores;
    }
}
