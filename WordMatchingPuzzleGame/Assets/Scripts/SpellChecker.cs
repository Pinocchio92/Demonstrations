using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellChecker : MonoBehaviour
{
    List<string> wordsToMatch;
    List<string> BonusWords;

    [SerializeField]
    bool isDiagonalAllowed = false;
    [SerializeField]
    bool isTurnAllowed = false;

    List<Cell> inReview = new List<Cell>();
    bool isChecking = false;

    public static event Action<Cell> OnletterHighlighted;
    public static event Action<bool, string> OnWordMatched;

    #region MonoBehaviours
    private void OnEnable()
    {
        Cell.OnCellClick += Cell_OnCellSelected;
        Cell.OnCellEnter += Cell_OnCellEnter;
        Cell.OnCellExit += Cell_OnCellExit;
        GameManager.Instance.OnLevelDataLoaded += Instance_OnLevelDataLoaded;
    }
    private void OnDisable()
    {
        Cell.OnCellClick -= Cell_OnCellSelected;
        Cell.OnCellEnter -= Cell_OnCellEnter;
        Cell.OnCellExit -= Cell_OnCellExit;
        GameManager.Instance.OnLevelDataLoaded -= Instance_OnLevelDataLoaded;
    }
    #endregion
    bool IsValid(Cell obj)
    {
        if (inReview.Count != 0)
        {
            if (inReview[inReview.Count - 1].coloumn != obj.coloumn && inReview[inReview.Count - 1].row != obj.row && !isDiagonalAllowed) //Ristrict Diagonal 
                return false;

            if (Mathf.Abs(inReview[inReview.Count - 1].coloumn - obj.coloumn) > 1 || Mathf.Abs(inReview[inReview.Count - 1].row - obj.row) > 1) // Ristrict Cell Skip

                return false;

            // allow only straight line : diff of col and row of last 2 index should be same to maintain straight line 
            if (!isTurnAllowed && (inReview.Count >= 2 && (Mathf.Abs(obj.coloumn - inReview[inReview.Count - 1].coloumn) != Mathf.Abs(inReview[inReview.Count - 1].coloumn - inReview[inReview.Count - 2].coloumn) || Mathf.Abs(obj.row - inReview[inReview.Count - 1].row) != Mathf.Abs(inReview[inReview.Count - 1].row - inReview[inReview.Count - 2].row))))
                return false;
        }
        return true;
    }
    public string getReversedString(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    #region ActionCallbacks
    private void Cell_OnCellExit(Cell obj)
    {
        string stringToMatch = string.Empty;
        isChecking = false;
        foreach (var item in inReview)
        {
            stringToMatch += item.letter;
        }
        bool wordMatched = false;
        foreach (var item in wordsToMatch)
        {
            if (item.ToUpper() == stringToMatch.ToUpper() || getReversedString(stringToMatch).ToUpper() == item.ToUpper())
            {
                wordMatched = true;
                stringToMatch = item;
                
            }
        }
        foreach (var item in inReview)
        {
            item.SetState(wordMatched ? CellState.UTILIZED: CellState.UNUSED);
        }
        inReview = new List<Cell>();
        OnWordMatched.Invoke(wordMatched, stringToMatch);
    }
    private void Cell_OnCellEnter(Cell obj)
    {
        if (isChecking  && IsValid(obj))
        {
            isChecking = true;
            inReview.Add (obj);
            obj.SetState(CellState.HIGHLIGHTED);
            OnletterHighlighted.Invoke(obj);
        }
    }
    private void Cell_OnCellSelected(Cell obj)
    {
        isChecking = true;
        inReview.Add(obj);
        obj.SetState(CellState.HIGHLIGHTED);
        OnletterHighlighted.Invoke(obj);
    }
    private void Instance_OnLevelDataLoaded(LevelData obj)
    {
        wordsToMatch = obj.wordsToPlace;
        BonusWords = obj.BonusWords;
    }
    #endregion
   
}
