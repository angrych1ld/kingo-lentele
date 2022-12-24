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
        string data = Encoding.ASCII.GetString(bytes);

#if UNITY_WEBGL && !UNITY_EDITOR
Debug.Log("Using local storage");
        WriteCurrentGame(data);
#else
        Debug.Log("Using file system");
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

            byte[] bytes = Encoding.ASCII.GetBytes(data);
            return GameState.Deserialize(bytes, 0);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return GameState.New("nėra", "nėra", "nėra");
        }
    }
}
