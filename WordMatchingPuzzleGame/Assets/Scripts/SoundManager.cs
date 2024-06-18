using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource backGroundAudioSource;

    [SerializeField]
    AudioSource OneShotAudioSource;
    [SerializeField]
    AudioClip WordHighlighted;
    [SerializeField]
    AudioClip WordMatched;
    [SerializeField]
    AudioClip WordNotMatched;




    private void OnEnable()
    {
        SpellChecker.OnletterHighlighted += SpellChecker_OnletterHighlighted;
        SpellChecker.OnWordMatched += SpellChecker_OnWordMatched;
    }

    private void OnDisable()
    {
        SpellChecker.OnletterHighlighted -= SpellChecker_OnletterHighlighted;
        SpellChecker.OnWordMatched -= SpellChecker_OnWordMatched;
    }

    private void SpellChecker_OnWordMatched(bool arg1, string arg2)
    {
        OneShotAudioSource.clip = arg1? WordMatched: WordNotMatched; 
        OneShotAudioSource.Play();
    }

    private void SpellChecker_OnletterHighlighted(Cell obj)
    {
        OneShotAudioSource.clip = WordHighlighted;
        OneShotAudioSource.Play();
    }

}
