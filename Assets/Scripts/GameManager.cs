using System;
using System.Collections;
using UnityEngine;

public enum GameStatus
{
    Runing,
    Menu
}

[RequireComponent(typeof(PlayerPrefsManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private StoreUI storeUi;
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
    private SoundManager soundManager;

    [Space(20)]
    [SerializeField]
    private float delayOnGameEnd = 3f;
    [SerializeField]
    private float musicVolumeInMenu = 1f;
    [SerializeField]
    private float musicVolumeInGameplay = 0.5f;

    [Space(20)]
    [SerializeField]
    private string NOT_ALL_CARROTS_KEY = "NotAllCollected";
    [SerializeField]
    private string LOSE_KEY = "LoseGame";
    [SerializeField]
    private string WIN_KEY = "WinGame";

    private LocalizationHandler localeHandler;
    private PlayerController playerController;
    private InteractionHandler interactionHandler;
    private int carrotsAll;
    private int carrotsPicked = 0;
    private int bonusesPicked = 0;
    private int bonusesAll = 0;
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
        uiManager.OnClearPrefs += ClearPlayerPrefs;
        uiManager.OnStartGame += StartGame;

        cameraController.enabled = false;
        levelInfoHandler.levels = prefsManager.LoadPlayerPrefs(levelInfoHandler.levels, out bonusesAll);
        UpdateMenuTexts();
        storeUi.UpdateBonuses(bonusesAll);
        soundManager.SetMusicVolume(musicVolumeInMenu);
        soundManager.PlayMusic();
    }

    private void Update()
    {
        uiManager.TimerUpdate(SetupTimer(Mathf.Clamp(secondsLeft, 0, secondsToPassLevel)), secondsLeft);
        secondsLeft -= Time.deltaTime;
        CheckLoseConditions();
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
        soundManager.SetMusicVolume(musicVolumeInGameplay);
        if (exitTimerCoroutine != null)
        {
            StopCoroutine(exitTimerCoroutine);
            exitTimerCoroutine = null;
        }
        gameplayUi.ResetStick();
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

        string storeMainText = FindByKey(storeUi.STORE_TEXT_KEY);

        uiManager.UpdatePopup(warningText, yesText, noText);
        mainMenu.UpdateMenu(startText, storeText, quitText);
        levelMenu.UpdateMenu(goText, resetText, backText, levelDifficultyText);
        levelMenu.levelName = levelNameText;
        gameplayUi.UpdateMenu(quitText);
        storeUi.UpdateMenu(storeMainText, quitText);

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
                soundManager.PlaySound(tileType.ToString());
                break;
            case TileType.Bonus:
                tilemapHandler.ChangeTile(new Vector3Int(currentTile.Position.y, -currentTile.Position.x, 0), tileType);
                uiManager.ScoreUpdate(carrotsPicked, carrotsAll, ++bonusesPicked);
                soundManager.PlaySound(tileType.ToString());
                break;
            case TileType.FinishPoint:
                CheckWinConditions();
                break;
            case TileType.ButtonOnOff:
                bool controlState = tilemapHandler.ChangeTile(new Vector3Int(currentTile.Position.y, -currentTile.Position.x, 0), tileType); //changes button on/off tiles
                tilemapHandler.ChangeInteractiveObstacle(controlState, (int)number);
                soundManager.PlaySound(tileType.ToString());
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
            int unlockedLevels = 0;
            if (status == GameStatus.Runing)
            {
                DisplayMessage(WIN_KEY);
            }
            if (exitTimerCoroutine == null)
            {
                exitTimerCoroutine = StartCoroutine(ExitTimerCoroutine(delayOnGameEnd));
            }
            bonusesAll += bonusesPicked;
            storeUi.UpdateBonuses(bonusesAll);
            foreach (var item in levelInfoHandler.levels)
            {
                if (item.IsOpen)
                {
                    unlockedLevels++;
                }
            }
            prefsManager.SavePlayerPrefs(bonusesAll, unlockedLevels);
            UnlockNextLevel(currentLevelId);
            status = GameStatus.Menu;
            soundManager.PlaySound("Win");
            soundManager.SetMusicVolume(musicVolumeInMenu);
        }
        else
        {
            DisplayMessage(NOT_ALL_CARROTS_KEY);
        }
    }

    private void CheckLoseConditions()
    {
        if (status == GameStatus.Runing && secondsLeft <= 0)
        {
            DisplayMessage(LOSE_KEY);
            if (exitTimerCoroutine == null)
            {
                exitTimerCoroutine = StartCoroutine(ExitTimerCoroutine(delayOnGameEnd));
            }
            status = GameStatus.Menu;
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
            return;
        }
    }

    private void ClearPlayerPrefs()
    {
        prefsManager.ClearPlayerPrefs(levelInfoHandler.levels, out bonusesAll);
    }
}
