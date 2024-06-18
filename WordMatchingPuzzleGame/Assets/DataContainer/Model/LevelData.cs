using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevelData", menuName = "WordSearch/Level")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public int rows;
    public int columns;
    public int timeInSeconds;
    public List<string> wordsToPlace;
    public List<string> BonusWords;
    public string[] data;
}
