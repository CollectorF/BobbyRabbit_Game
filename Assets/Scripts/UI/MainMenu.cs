using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    internal LocalizationHandler localizationHandler;
    [SerializeField]
    internal TextMeshProUGUI startGameButton;
    [SerializeField]
    internal TextMeshProUGUI storeButton;
    [SerializeField]
    internal TextMeshProUGUI quitButton;

    [Space(20)]
    [SerializeField]
    internal string START_GAME_KEY = "StartGame";
    [SerializeField]
    internal string STORE_KEY = "Store";
    [SerializeField]
    internal string QUIT_KEY = "Quit";

    internal Action<string> OnLocaleButtonClick;
    internal Action<string> OnQuitButtonClick;
    internal Action OnStartButtonClick;

    public void UpdateMenu(string start, string store, string quit)
    {
        startGameButton.text = start;
        storeButton.text = store;
        quitButton.text = quit;
    }

    public void OnLocaleClick(string locale)
    {
        OnLocaleButtonClick?.Invoke(locale);
    }

    public void OnStartClick()
    {
        OnStartButtonClick?.Invoke();
    }

    public void OnQuitClick()
    {
        OnQuitButtonClick?.Invoke(tag);
    }
}
