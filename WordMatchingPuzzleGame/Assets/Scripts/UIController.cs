using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenuScreen;
    [SerializeField]
    GameObject LevelScreen;
    [SerializeField]
    GameObject GameScreen;
    [SerializeField]
    GameObject LevelFinishScreen;
    [SerializeField]
    Transform SearchingWordsParent;
    [SerializeField]
    Transform HighlightedLettersParent;
    [SerializeField]
    Transform LevelViewContainer;

    List<SearchingWord> searchWords;
    private void OnEnable()
    {
        SpellChecker.OnletterHighlighted += SpellChecker_OnletterHighlighted;
        SpellChecker.OnWordMatched += SpellChecker_OnWordMatched;
        GameManager.Instance.OnGameStateChanged += Instance_OnGameStateChanged;
        GameManager.Instance.OnLevelDataLoaded += Instance_OnLevelDataLoaded;
    }
    private void OnDisable()
    {
        SpellChecker.OnletterHighlighted -= SpellChecker_OnletterHighlighted;
        SpellChecker.OnWordMatched -= SpellChecker_OnWordMatched;
        GameManager.Instance.OnGameStateChanged += Instance_OnGameStateChanged; 
        GameManager.Instance.OnLevelDataLoaded += Instance_OnLevelDataLoaded;
    }
    private void Start()
    {
        LoadLevels();
    }
    void LoadLevels()
    {
        for (int i = 0; i < DataHandler.Instance.GetLevelCount(); i++)
        {
            var level = Instantiate(LevelViewContainer.GetChild(0), LevelViewContainer);
            level.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Level " + (i + 1);
            level.gameObject.SetActive(true);
        }
        
    }
    void UpdateSearchingWordsUI(List<string> wordsToFind)
    {
        searchWords = new List<SearchingWord>();
        foreach (var item in wordsToFind)
        {
            var sw = CreateWord();
            sw.InitializeWord(item);
            searchWords.Add(sw);
        }
            
       
    }
    SearchingWord CreateWord()
    {
        for (int i = 1; i < SearchingWordsParent.childCount; i++)
        {
            if (!SearchingWordsParent.GetChild(i).gameObject.activeSelf && SearchingWordsParent.GetChild(i).TryGetComponent<SearchingWord>(out var sw))
            {
                return sw;
            }
        }
        return Instantiate(SearchingWordsParent.GetChild(0), SearchingWordsParent).GetComponent<SearchingWord>();
    }
    void ReleaseSearchingWords()
    {
        for (int i = 0; i < SearchingWordsParent.childCount; i++)
            SearchingWordsParent.GetChild(i).gameObject.SetActive(false);
    }
    private void Instance_OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.None:
                break;
            case GameState.MainMenu:
                MainMenuScreen.SetActive(true);
                LevelScreen.SetActive(false);
                GameScreen.SetActive(false);
                LevelFinishScreen.SetActive(false);
                break;
            case GameState.LevelSelection:
                MainMenuScreen.SetActive(false);
                LevelScreen.SetActive(true);
                GameScreen.SetActive(false);
                LevelFinishScreen.SetActive(false);
                break;
            case GameState.LevelInProgress:
                MainMenuScreen.SetActive(false);
                LevelScreen.SetActive(false);
                GameScreen.SetActive(true);
                LevelFinishScreen.SetActive(false);
                break;
            case GameState.LevelPaused:
                break;
            case GameState.LevelCompleted:
                ReleaseSearchingWords();
                MainMenuScreen.SetActive(false);
                LevelScreen.SetActive(false);
                GameScreen.SetActive(false);
                LevelFinishScreen.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void OnStartGameButtonClick()
    {
        GameManager.Instance.SetGameState(GameState.LevelSelection);
    }
    public void OnLevelSelected(Transform T)
    {
        GameManager.Instance.InitializeLevel(T.GetSiblingIndex()-1);
    }
    public void OnBackButtonClick()
    {
        MainMenuScreen.SetActive(true);
        LevelScreen.SetActive(false);
        GameScreen.SetActive(false);
    }
    TMPro.TMP_Text CreateHighlightedLetter()
    {
        for (int i = 1; i < HighlightedLettersParent.childCount; i++)
        {
            if (!HighlightedLettersParent.GetChild(i).gameObject.activeSelf && HighlightedLettersParent.GetChild(i).GetChild(0).TryGetComponent<TMPro.TMP_Text>(out var text))
            {
                return text;
            }
        }
        return Instantiate(HighlightedLettersParent.GetChild(0), HighlightedLettersParent).GetChild(0).GetComponent<TMPro.TMP_Text>();
    }

    #region ActionCallbacks
    private void SpellChecker_OnWordMatched(bool ismatched, string word)
    {
        if (ismatched)
        {
            foreach (var item in searchWords)
            {
                if (!item.isCrossed && item.word.ToUpper() == word.ToUpper())
                {
                    item.MarkCrossed(true);
                    break;
                }
            }
        }    
        
        for (int i = 0; i < HighlightedLettersParent.childCount; i++)
                HighlightedLettersParent.GetChild(i).gameObject.SetActive(false);

        
    }
    private void SpellChecker_OnletterHighlighted(Cell obj)
    {
        var text = CreateHighlightedLetter();
        text.text = obj.letter.ToString();
        text.transform.parent.gameObject.SetActive(true);
    }
    private void Instance_OnLevelDataLoaded(LevelData obj)
    {
        UpdateSearchingWordsUI(obj.wordsToPlace);
    }
    #endregion
    
}
