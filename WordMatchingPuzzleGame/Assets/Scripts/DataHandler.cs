using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    private static DataHandler instance = null;
    public static DataHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataHandler>();
            }
            return instance;
        }
    }
    [SerializeField]
    GameLevels gameLeveldata;
    public LevelData GetLevelData(int levelIndex)
    {
        return gameLeveldata.Levels[levelIndex];
    }
    public int GetLevelCount()
    {
        return gameLeveldata.Levels.Count;
    }
}
