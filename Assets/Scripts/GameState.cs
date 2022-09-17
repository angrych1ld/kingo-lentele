using System;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct GameState
{
    public const int ROUNDS_PER_PLAYER = 10;

    public Guid guid;
    public PlayerState[] players;

    public float GetCompletion()
    {
        int total = 0;
        int completed = 0;
        for (int i = 0; i < players.Length; i++)
        {
            for (int j = 0; j < players[i].rounds.Length; j++)
            {
                total++;
                if (players[i].rounds[j].PointsAssigned())
                {
                    completed++;
                }
            }
        }

        if (total == 0)
        {
            return 0;
        }

        return ((float)completed) / total;
    }

    public static GameState Deserialize(string data)
    {
        try
        {
            return JsonUtility.FromJson<GameState>(data);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }


        return New("nėra", "nėra", "nėra");
    }

    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }

    public static GameState New(params string[] playerNames)
    {
        return new GameState
        {
            guid = Guid.NewGuid(),
            players = playerNames.Select(n => new PlayerState
            {
                playerName = n,
                rounds = Enumerable.Repeat(
                    new RoundState { points = new int[playerNames.Length] },
                    ROUNDS_PER_PLAYER).ToArray()
            }).ToArray()
        };
    }
}

[System.Serializable]
public struct PlayerState
{
    public string playerName;
    public RoundState[] rounds;
}

[System.Serializable]
public struct RoundState
{
    public int[] points;

    public bool PointsAssigned()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] != 0)
            {
                return true;
            }
        }
        return false;
    }
}