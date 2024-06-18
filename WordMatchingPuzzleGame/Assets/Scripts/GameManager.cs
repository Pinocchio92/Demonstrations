using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    public event Action<GameState> OnGameStateChanged;
    public event Action<LevelData> OnLevelDataLoaded;

    public GameState currentGameState { get; private set; }
    
    LevelData currentLevelData;
    int wordsFounnd;
    #region MonoBehaviours
    private void OnEnable()
    {
        SpellChecker.OnWordMatched += SpellChecker_OnWordMatched;
    }
    private void OnDisable()
    {
        SpellChecker.OnWordMatched -= SpellChecker_OnWordMatched;
    }
    private void Start()
    {
        OnGameStateChanged.Invoke(GameState.MainMenu);
    }
    #endregion
    public void InitializeLevel(int levelIndex)
    {
        currentLevelData = DataHandler.Instance.GetLevelData(levelIndex);
        OnLevelDataLoaded.Invoke(currentLevelData);
        wordsFounnd = 0;
        SetGameState(GameState.LevelInProgress);
    }
    #region ActionCallbacks
    private void SpellChecker_OnWordMatched(bool arg1, string arg2)
    {
        if (arg1)
            wordsFounnd++;
        if (wordsFounnd >= currentLevelData.wordsToPlace.Count)
        {
            SetGameState(GameState.LevelCompleted);
        }
    }
    public void SetGameState(GameState state)
    {
        if (state != currentGameState)
        {
            currentGameState = state;
            OnGameStateChanged.Invoke(currentGameState);
        }
    }
    #endregion

}

public enum GameState
{
    None = -1,
    MainMenu = 0,
    LevelSelection = 1,
    LevelInProgress = 2,
    LevelPaused =3,
    LevelCompleted = 4
}
