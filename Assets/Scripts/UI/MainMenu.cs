using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    internal TMP_Text startGameButton;
    [SerializeField]
    internal TMP_Text storeButton;
    [SerializeField]
    internal TMP_Text quitButton;

    [Space(20)]
    [SerializeField]
    internal string START_GAME_KEY = "StartGame";
    [SerializeField]
    internal string STORE_KEY = "Store";
    [SerializeField]
    internal string QUIT_KEY = "Quit";

    internal Action<string> OnLocaleButtonClick;

    public void OnLocaleClick(string locale)
    {
        OnLocaleButtonClick?.Invoke(locale);
    }
    public void UpdateMenu(string start, string store, string quit)
    {
        startGameButton.text = start;
        storeButton.text = store;
        quitButton.text = quit;
    }
}
