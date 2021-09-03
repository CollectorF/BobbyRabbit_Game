using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField]
    internal TMP_Text startButton;
    [SerializeField]
    internal TMP_Text clearProgressButton;
    [SerializeField]
    internal TMP_Text backButton;
    [SerializeField]
    internal TMP_Text levelInfo;

    [Space(20)]
    [SerializeField]
    internal string START_KEY = "Start";
    [SerializeField]
    internal string RESET_KEY = "ResetProgress";
    [SerializeField]
    internal string BACK_KEY = "Back";

    private LevelLoader levelLoader;
    internal Action OnBackButtonClick;

    public void OnBackClick()
    {
        OnBackButtonClick?.Invoke();
    }
    public void UpdateMenu(string start, string clear, string back)
    {
        startButton.text = start;
        clearProgressButton.text = clear;
        backButton.text = back;
    }
    private void Awake()
    {

    }
}
