using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static void SaveCurrentGame(GameState state)
    {
        File.WriteAllText(Application.persistentDataPath + "/save.json", state.Serialize());
    }

    public static GameState LoadCurrentGame()
    {
        string data = null;
        try
        {
            data = File.ReadAllText(Application.persistentDataPath + "/save.json");
        }
        catch
        {
            return GameState.New("nėra", "nėra", "nėra");
        }
        return GameState.Deserialize(data);
    }
}
