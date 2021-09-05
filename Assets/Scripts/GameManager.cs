using System;
using System.Collections;
using UnityEngine;

public enum GameStatus
{
    Runing,
    Menu
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
    private GameplayUI gameplayUi;
    [SerializeField]
    private TilemapHandler tilemapHandler;
    [SerializeField]
    private LevelLoader levelLoaderMain;
    [SerializeField]
    private LevelLoader levelLoaderBackground;
    [SerializeField]
    private LevelInfoHandler levelInfoHandler;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private float delayOnGameEnd = 3f;

    [Space(20)]
    [SerializeField]
    private string NOT_ALL_CARROTS_KEY = "NotAllCollected";
    [SerializeField]
    private string LOOSE_KEY = "LooseGame";
    [SerializeField]
    private string WIN_KEY = "WinGame";

    private LocalizationHandler localeHandler;
    private PlayerController playerController;
    private InteractionHandler interactionHandler;
    private int carrotsAll;
    private int carrotsPicked = 0;
    private int bonusesPicked = 0;
    private float secondsLeft;
    private float secondsToPassLevel;
    private GameStatus status;
    private PlayerPrefsManager prefsManager;
    private int currentLevelId;
    private Coroutine exitTimerCoroutine;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        interactionHandler = player.GetComponent<InteractionHandler>();
        localeHandler = GetComponent<LocalizationHandler>();
        prefsManager = GetComponent<PlayerPrefsManager>();

        status = GameStatus.Menu;
    }
    private void Start()
    {
        levelLoaderMain.OnLevelLoad += SetStartPosition;
        playerController.OnNoWay += DisplayMessage;
        interactionHandler.OnInteraction += ProcessInteraction;
        localeHandler.OnLocaleDictFill += UpdateMenuTexts;
        uiManager.OnClearPrefs += prefsManager.ClearPlayerPrefs;
        uiManager.OnStartGame += StartGame;

        UpdateMenuTexts();
        cameraController.enabled = false;
    }

    private void Update()
    {
        uiManager.TimerUpdate(SetupTimer(Mathf.Clamp(secondsLeft, 0, secondsToPassLevel)), secondsLeft);
        secondsLeft -= Time.deltaTime;
        CheckLooseConditions();
    }

    private void StartGame(int levelId)
    {
        carrotsPicked = 0;
        bonusesPicked = 0;
        currentLevelId = levelId;
        levelLoaderMain.SetupLevel(currentLevelId);
        levelLoaderBackground.SetupLevel(currentLevelId);
        tilemapHandler.SetObstacleInitialState(levelLoaderMain.map, levelInfoHandler.levels, currentLevelId);
        status = GameStatus.Runing;
        carrotsAll = levelLoaderMain.map.CarrotQuantity;
        secondsToPassLevel = levelInfoHandler.levels[currentLevelId].Timer;
        secondsLeft = secondsToPassLevel;
        uiManager.ScoreUpdate(carrotsPicked, carrotsAll, bonusesPicked);
        cameraController.enabled = true;
        cameraController.SetInitialCameraPosition();
    }

    private void SetStartPosition()
    {
        player.SetActive(false);
        playerController.SetPlayerInitialPosition();
        player.SetActive(true);
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

        uiManager.UpdatePopup(warningText, yesText, noText);
        mainMenu.UpdateMenu(startText, storeText, quitText);
        levelMenu.UpdateMenu(goText, resetText, backText, levelDifficultyText);
        levelMenu.levelName = levelNameText;
        gameplayUi.UpdateMenu(quitText);

        foreach (var item in levelInfoHandler.levels)
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
                CheckWinConditions();
                break;
            case TileType.ButtonOnOff:
                bool controlState = tilemapHandler.ChangeTile(new Vector3Int(currentTile.Position.y, -currentTile.Position.x, 0), tileType); //changes button on/off tiles
                tilemapHandler.ChangeInteractiveObstacle(controlState, (int)number);
                break;
            default:
                break;
        }
    }

    private IEnumerator ExitTimerCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiManager.ActivateLevelMenu();
        levelLoaderMain.map = null;
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

    private void CheckWinConditions()
    {
        if (carrotsPicked == carrotsAll && secondsLeft > 0)
        {
            DisplayMessage(WIN_KEY);
            UnlockNextLevel(currentLevelId);
            if (exitTimerCoroutine == null)
            {
                exitTimerCoroutine = StartCoroutine(ExitTimerCoroutine(delayOnGameEnd));
            }
        }
        else
        {
            DisplayMessage(NOT_ALL_CARROTS_KEY);
        }
    }

    private void CheckLooseConditions()
    {
        if (status == GameStatus.Runing && secondsLeft <= 0)
        {
            DisplayMessage(LOOSE_KEY);
            if (exitTimerCoroutine == null)
            {
                exitTimerCoroutine = StartCoroutine(ExitTimerCoroutine(delayOnGameEnd));
            }
        }
    }

    private void UnlockNextLevel(int currentLevel)
    {
        try
        {
            levelInfoHandler.levels[currentLevel + 1].IsOpen = true;
        }
        catch (Exception)
        {

        }
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
