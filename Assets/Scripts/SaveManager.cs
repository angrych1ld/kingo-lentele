using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Text;

public class SaveManager
{
    [DllImport("__Internal")]
    private static extern void WriteCurrentGame(string str);

    [DllImport("__Internal")]
    private static extern string ReadCurrentGame();

    [DllImport("__Internal")]
    private static extern void WriteHistoricalData(string str);

    [DllImport("__Internal")]
    private static extern string ReadHistoricalData();

    public static void SaveCurrentGame(GameState state)
    {
        byte[] bytes = state.Serialize();
        string data = Convert.ToBase64String(bytes);
        Debug.Log("Writing save " + data);
#if UNITY_WEBGL && !UNITY_EDITOR
        WriteCurrentGame(data);
#else
        File.WriteAllText(Application.persistentDataPath + "/currentGame.data", data);
#endif
    }

    public static GameState LoadCurrentGame()
    {
        string data = null;

        try
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            data = ReadCurrentGame();
#else
            data = File.ReadAllText(Application.persistentDataPath + "/currentGame.data").Replace(Environment.NewLine, "");
#endif

            byte[] bytes = Convert.FromBase64String(data);
            return GameState.Deserialize(bytes, 0);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return GameState.New("nėra", "nėra", "nėra");
        }
    }
}
