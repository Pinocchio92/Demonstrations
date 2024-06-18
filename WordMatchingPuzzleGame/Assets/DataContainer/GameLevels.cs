using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevelDataContainer", menuName = "WordSearch/LevelDataContainer")]
public class GameLevels : ScriptableObject
{
    public List<LevelData> Levels;
}