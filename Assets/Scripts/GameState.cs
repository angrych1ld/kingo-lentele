﻿using System;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;

[System.Serializable]
public struct GameState
{
    public const int ROUNDS_PER_PLAYER = 10;

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

    public static GameState Deserialize(byte[] data, int start)
    {
        using (var stream = new MemoryStream(data, start, data.Length - start, false))
        {
            stream.Seek(start, SeekOrigin.Begin);
            using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
            {
                int playerCount = reader.ReadByte();

                GameState state = new GameState
                {
                    players = new PlayerState[playerCount]
                };

                for (int i = 0; i < playerCount; i++)
                {
                    state.players[i].playerName = reader.ReadString();

                    state.players[i].rounds = new RoundState[ROUNDS_PER_PLAYER];

                    for (int j = 0; j < ROUNDS_PER_PLAYER; j++)
                    {
                        state.players[i].rounds[j].points = new int[playerCount];
                        for (int k = 0; k < playerCount; k++)
                        {
                            state.players[i].rounds[j].points[k] = reader.ReadInt16();
                        }
                    }
                }

                return state;
            }
        }
    }

    public byte[] Serialize()
    {
        using (var stream = new MemoryStream())
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                int playerCount = players.Length;
                writer.Write((byte)playerCount);

                for (int i = 0; i < playerCount; i++)
                {
                    writer.Write(players[i].playerName);
                    for (int j = 0; j < ROUNDS_PER_PLAYER; j++)
                    {
                        for (int k = 0; k < playerCount; k++)
                        {
                            writer.Write((short)players[i].rounds[j].points[k]);
                        }
                    }
                }

                byte[] bytes = stream.ToArray();
                Debug.Log("Serialized bytes " + bytes.Length + " " + string.Join(", ", bytes));
                return bytes;
            }
        }
    }

    public static GameState New(params string[] playerNames)
    {
        return new GameState
        {
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