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

    [Space(20)]
    [SerializeField]
    internal string START_GAME_KEY = "StartGame";
    [SerializeField]
    internal string STORE_KEY = "Store";

    internal Action<string> OnLocaleButtonClick;
    internal Action<GameState> OnStartButtonClick;
    internal Action<GameState> OnStoreButtonClick;

    public void UpdateMenu(string start, string store)
    {
        startGameButton.text = start;
        storeButton.text = store;
    }

    public void OnLocaleClick(string locale)
    {
        OnLocaleButtonClick?.Invoke(locale);
    }

    public void OnStartClick()   
    {
        var state = GameState.LevelMenu;
        OnStartButtonClick?.Invoke(state);
    }

    public void OnStoreClick()
    {
        var state = GameState.StoreMenu;
        OnStoreButtonClick?.Invoke(state);
    }
}
