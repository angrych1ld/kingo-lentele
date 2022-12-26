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

    public static byte[] GetHistoricalData()
    {
        string historicalDataString = "";
        try
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        historicalDataString = ReadHistoricalData();
#else
            historicalDataString = File.ReadAllText(Application.persistentDataPath + "/historicalData.data");
#endif
        }
        catch
        {
            Debug.LogWarning("Couldnt load historical data");
            historicalDataString = "";
        }
        byte[] historicalData = Convert.FromBase64String(historicalDataString);
        return historicalData;
    }

    public static void AppendHistoricalGame(GameState state)
    {
        string historicalDataString = "";
        try
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        historicalDataString = ReadHistoricalData();
#else
            historicalDataString = File.ReadAllText(Application.persistentDataPath + "/historicalData.data");
#endif
        }
        catch
        {
            Debug.LogWarning("Couldnt load historical data");
            historicalDataString = "";
        }

        byte[] historicalData = Convert.FromBase64String(historicalDataString);
        byte[] newData = state.Serialize();
        byte[] resultData = new byte[historicalData.Length + newData.Length + 4];
        Array.Copy(BitConverter.GetBytes(newData.Length), resultData, 4);
        Array.Copy(newData, 0, resultData, 4, newData.Length);
        Array.Copy(historicalData, 0, resultData, 4 + newData.Length, historicalData.Length);
        string resultDataString = Convert.ToBase64String(resultData);

#if UNITY_WEBGL && !UNITY_EDITOR
        WriteHistoricalData(resultDataString);
#else
        File.WriteAllText(Application.persistentDataPath + "/historicalData.data", resultDataString);
#endif
    }

    public static void SaveCurrentGame(GameState state)
    {
        byte[] bytes = state.Serialize();
        string data = Convert.ToBase64String(bytes);
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
