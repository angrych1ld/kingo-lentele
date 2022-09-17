using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Game Config")]
public class GameConfig : ScriptableObject
{
    public RoundConfig[] rounds = new RoundConfig[10];

    [System.Serializable]
    public class RoundConfig
    {
        public int tickCount;
        public int tickValue;
    }
}
