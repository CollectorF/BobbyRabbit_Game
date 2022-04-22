using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject storeUI;
    [SerializeField]
    private GameObject levelMenu;
    [SerializeField]
    private GameObject gameplayUI;
    [SerializeField]
    private GameObject popup;
    [SerializeField]
    private LevelInfoHandler levelInfoHandler;

    private MainMenu mainMenuManager;
    private LevelMenu levelMenuManager;
    private GameplayUI gameplayUiManager;
    private Canvas canvas;
    private string popupCaller;

    internal event Action<string> OnDisplayTextMessage;
    internal event Action<string, float> OnUpdateTimer;
    internal event Action OnClearPrefs;
    internal event Action<int> OnStartGame;

    private void Awake()
    {
        mainMenuManager = mainMenu.GetComponent<MainMenu>();
        levelMenuManager = levelMenu.GetComponent<LevelMenu>();
        gameplayUiManager = gameplayUI.GetComponent<GameplayUI>();
        canvas = GetComponentInChildren<Canvas>();

        // Popup control
        gameplayUiManager.OnQuitButtonClick += HandlePopup;
        levelMenuManager.OnClearButtonClick += HandlePopup;

        //mainMenuManager.OnStartButtonClick += ActivateLevelMenu;
        levelMenuManager.OnStartButtonClick += ActivateGameplayUI;
    }

    private void Start()
    {
        ActivateMainMenu();
    }

    public void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
        storeUI.SetActive(false);
        levelMenu.SetActive(false);
        gameplayUI.SetActive(false);
    }

    public void ActivateLevelMenu()
    {
        mainMenu.SetActive(false);
        storeUI.SetActive(false);
        levelMenu.SetActive(true);
        gameplayUI.SetActive(false);
        UpdateLevelList();
    }

    public void ActivateGameplayUI(int? id)
    {
        mainMenu.SetActive(false);
        storeUI.SetActive(false);
        levelMenu.SetActive(false);
        gameplayUI.SetActive(true);
        OnStartGame?.Invoke((int)id);
        levelMenuManager.DestroyLevelList();
    }

    public void ActivateStoreUI()
    {
        mainMenu.SetActive(false);
        storeUI.SetActive(true);
        levelMenu.SetActive(false);
        gameplayUI.SetActive(false);
    }

    public void HandlePopup(string caller)
    {
        popup.SetActive(true);
        popupCaller = caller;
    }

    internal void UpdateLevelList()
    {
        levelMenuManager.FillLevelList(levelInfoHandler.levels);
    }

    // Transfer of events from GameManager to GameplayUI

    internal void DisplayMessage(string msg)
    {
        OnDisplayTextMessage?.Invoke(msg);
    }

    public void ScoreUpdate(int carrots, int carrotsAll, int bonuses)
    {
        gameplayUiManager.UpdateScore(carrots, carrotsAll, bonuses);
    }

    public void TimerUpdate(string timer, float timeLeft)
    {
        OnUpdateTimer?.Invoke(timer, timeLeft);
    }
}
