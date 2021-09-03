using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject levelMenu;
    [SerializeField]
    private GameObject gameplayUI;
    [SerializeField]
    private GameObject popup;
    [SerializeField]
    internal TMP_Text popupText;
    [SerializeField]
    internal TMP_Text popupYes;
    [SerializeField]
    internal TMP_Text popupNo;

    [Space(20)]
    [SerializeField]
    internal string POPUP_WARNING_KEY = "PopupWarning";
    [SerializeField]
    internal string POPUP_YES_KEY = "Yes";
    [SerializeField]
    internal string POPUP_NO_KEY = "No";

    private MainMenu mainMenuManager;
    private LevelMenu levelMenuManager;
    private string popupCaller;

    internal event Action<string> OnDisplayTextMessage;
    internal event Action<int, int, int> OnUpdateScore;
    internal event Action<string, float> OnUpdateTimer;

    private void Awake()
    {
        mainMenuManager = mainMenu.GetComponent<MainMenu>();
        levelMenuManager = levelMenu.GetComponent<LevelMenu>();
        mainMenuManager.OnQuitButtonClick += ShowPopup;
        levelMenuManager.OnBackButtonClick += ActivateMainMenu;
    }

    private void Start()
    {
        ActivateMainMenu();
    }

    public void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
        levelMenu.SetActive(false);
        gameplayUI.SetActive(false);
        popup.SetActive(false);
    }

    public void ActivateLevelMenu()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(true);
        gameplayUI.SetActive(false);
        popup.SetActive(false);
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(false);
        gameplayUI.SetActive(true);
        popup.SetActive(false);
    }

    internal void DisplayMessage(string msg)
    {
        OnDisplayTextMessage?.Invoke(msg);
    }

    public void ScoreUpdate(int carrots, int carrotsAll, int bonuses)
    {
        OnUpdateScore?.Invoke(carrots, carrotsAll, bonuses);
    }
    public void TimerUpdate(string timer, float timeLeft)
    {
        OnUpdateTimer?.Invoke(timer, timeLeft);
    }

    public void ShowPopup(string caller)
    {
        popup.SetActive(true);
        popupCaller = caller;
    }

    public void HidePopup()
    {
        popup.SetActive(false);
    }

    public void UpdatePopup(string warning, string yes, string no)
    {
        popupText.text = warning;
        popupYes.text = yes;
        popupNo.text = no;
    }
    public void OnPopupYes()
    {
        if (popupCaller == "MainMenu")
        {
            Application.Quit();
        }
        if (popupCaller == "LevelMenu")
        {

        }
    }
    public void OnPopupNo()
    {
        HidePopup();
    }
}
