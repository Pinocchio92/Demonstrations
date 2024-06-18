using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    [SerializeField]
    TMP_Text displayedText;
    [SerializeField]
    Image crossLine;
    
    [HideInInspector]
    public string word { get; private set; }
    [HideInInspector]
    public bool isCrossed = false;
    public void InitializeWord(string wrd)
    {
        word = wrd.ToUpper();
        displayedText.text = word;
        crossLine.gameObject.SetActive(false);
        isCrossed = false;
        gameObject.SetActive(true);
    }

    public void MarkCrossed(bool value)
    {
        isCrossed = true;
        crossLine.gameObject.SetActive(value);
    }

}
