using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GameStatus
{
    Runing,
    PlayerWin,
    PlayerLoose
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private LevelMenu levelMenu;
    [SerializeField]
    private TilemapHandler tilemapHandler;
    [SerializeField]
    private LevelLoader levelLoaderMain;

    [Space(20)]
    [SerializeField]
    private string NOT_ALL_CARROTS_KEY = "NotAllCollected";
    [SerializeField]
    private string LOOSE_KEY = "LooseGame";
    [SerializeField]
    private string WIN_KEY = "WinGame";

    private LocalizationHandler localeHandler;
    private PlayerController playerController;
    private InteractionHandler interactionProcessor;
    private int carrotsAll;
    private int carrotsPicked = 0;
    private int bonusesPicked = 0;
    private float secondsLeft;
    private float secondsToPassLevel;
    private GameStatus status;
    private PlayerPrefsManager prefsManager;

    //internal event Action<int> OnButtonInteraction;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        interactionProcessor = player.GetComponent<InteractionHandler>();
        localeHandler = GetComponent<LocalizationHandler>();
        prefsManager = GetComponent<PlayerPrefsManager>();

        playerController.OnNoWay += DisplayMessage;
        interactionProcessor.OnInteraction += ProcessInteraction;
        localeHandler.OnLocaleDictFill += UpdateMenuTexts;
        uiManager.OnClearPrefs += prefsManager.ClearPlayerPrefs;
    }
    private void Start()
    {
        status = GameStatus.Runing;
        carrotsAll = levelLoaderMain.map.CarrotQuantity;
        uiManager.ScoreUpdate(carrotsPicked, carrotsAll, bonusesPicked);
        secondsToPassLevel = levelLoaderMain.levels[0].Timer;
        secondsLeft = secondsToPassLevel;
        UpdateMenuTexts();
    }

    private void Update()
    {
        uiManager.TimerUpdate(SetupTimer(Mathf.Clamp(secondsLeft, 0, secondsToPassLevel)), secondsLeft);
        secondsLeft -= Time.deltaTime;
        status = CheckLooseConditions();
    }

    private void UpdateMenuTexts()
    {
        string startText = FindByKey(mainMenu.START_GAME_KEY);
        string storeText = FindByKey(mainMenu.STORE_KEY);
        string quitText = FindByKey(mainMenu.QUIT_KEY);

        string warningText = FindByKey(uiManager.POPUP_WARNING_KEY);
        string yesText = FindByKey(uiManager.POPUP_YES_KEY);
        string noText = FindByKey(uiManager.POPUP_NO_KEY);

        string goText = FindByKey(levelMenu.START_KEY);
        string resetText = FindByKey(levelMenu.RESET_KEY);
        string backText = FindByKey(levelMenu.BACK_KEY);
        string levelNameText = FindByKey(levelMenu.LEVEL_NAME_KEY);
        string levelDifficultyText = FindByKey(levelMenu.LEVEL_DIFFICULTY_KEY);
        string levelEasyText = FindByKey(levelMenu.LEVEL_EASY_KEY);
        string levelMediumText = FindByKey(levelMenu.LEVEL_MEDIUM_KEY);
        string levelHardText = FindByKey(levelMenu.LEVEL_HARD_KEY);

        mainMenu.UpdateMenu(startText, storeText, quitText);
        uiManager.UpdatePopup(warningText, yesText, noText);
        levelMenu.UpdateMenu(goText, resetText, backText, levelDifficultyText);
        levelMenu.levelName = levelNameText;

        foreach (var item in levelLoaderMain.levels)
        {
            switch (item.Difficulty)
            {
                case Difficulty.Easy:
                    item.DifficultyString = levelEasyText;
                    break;
                case Difficulty.Medium:
                    item.DifficultyString = levelMediumText;
                    break;
                case Difficulty.Hard:
                    item.DifficultyString = levelHardText;
                    break;
                default:
                    break;
            }
        }
    }

    private void DisplayMessage(string key)
    {
        string msg = FindByKey(key);
        uiManager.DisplayMessage(msg);
    }

    private void ProcessInteraction(MapTile currentTile, TileType tileType, int? number)
    {
        switch (tileType)
        {
            case TileType.Carrot:
                tilemapHandler.ChangeTile(new Vector3Int(currentTile.Position.y, -currentTile.Position.x, 0), tileType);
                uiManager.ScoreUpdate(++carrotsPicked, carrotsAll, bonusesPicked);
                break;
            case TileType.Bonus:
                tilemapHandler.ChangeTile(new Vector3Int(currentTile.Position.y, -currentTile.Position.x, 0), tileType);
                uiManager.ScoreUpdate(carrotsPicked, carrotsAll, ++bonusesPicked);
                break;
            case TileType.FinishPoint:
                status = CheckWinConditions();
                if (status == GameStatus.PlayerWin)
                {
                    DisplayMessage(WIN_KEY);
                }
                else
                {
                    DisplayMessage(NOT_ALL_CARROTS_KEY);
                }
                break;
            case TileType.ButtonOnOff:
                bool controlState = tilemapHandler.ChangeTile(new Vector3Int(currentTile.Position.y, -currentTile.Position.x, 0), tileType); //changes button on/off tiles
                tilemapHandler.ChangeInteractiveObstacle(controlState, (int)number);
                break;
            default:
                break;
        }
    }

    private string FindByKey(string key)
    {
        localeHandler.dictionary.TryGetValue(key, out string value);
        return value;
    }

    private string SetupTimer(float seconds)
    {
        float minutesLeft = TimeSpan.FromSeconds(seconds).Minutes;
        float secondsLeft = TimeSpan.FromSeconds(seconds).Seconds;
        string timer = string.Format("{0:00}:{1:00}", minutesLeft, secondsLeft);
        return timer;
    }

    private GameStatus CheckWinConditions()
    {
        if (carrotsPicked == carrotsAll && secondsLeft > 0)
        {
            status = GameStatus.PlayerWin;
        }
        else
        {
            status = GameStatus.Runing;
        }
        return status;
    }

    private GameStatus CheckLooseConditions()
    {
        if (secondsLeft == 0)
        {
            DisplayMessage(LOOSE_KEY);
            status = GameStatus.PlayerLoose;
        }
        return status;
    }

    //private void CheckInteraction(int number)
    //{
    //    if (levelLoaderMain.map.Buttons[number].IsOn)
    //    {
    //        MapTile obstacleToInteract = levelLoaderMain.map.Obstacles[(int)number];
    //        tilemapHandler.ChangeTile(new Vector3Int(obstacleToInteract.Position.y, -obstacleToInteract.Position.x, 0), obstacleToInteract.Type);
    //    }
    //}
}
