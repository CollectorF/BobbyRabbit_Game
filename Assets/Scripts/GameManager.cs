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

    private CaptionHandler captionHandler;
    private PlayerController playerController;
    private InteractionHandler interactionProcessor;
    private int carrotsAll;
    private int carrotsPicked = 0;
    private int bonusesPicked = 0;
    private float secondsLeft;
    private float secondsToPassLevel;
    private GameStatus status;



    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        interactionProcessor = player.GetComponent<InteractionHandler>();
        captionHandler = GetComponent<CaptionHandler>();

        status = GameStatus.Runing;
        playerController.OnNoWay += DisplayMessage;
        interactionProcessor.OnInteraction += ProcessInteraction;
        carrotsAll = levelLoaderMain.map.carrotQuantity;
        uiManager.UpdateScore(carrotsPicked, carrotsAll, bonusesPicked);
        secondsToPassLevel = levelLoaderMain.level.timer;
        secondsLeft = secondsToPassLevel;
    }

    private void Update()
    {
        uiManager.UpdateTimer(SetupTimer(Mathf.Clamp(secondsLeft, 0, secondsToPassLevel)), secondsLeft);
        secondsLeft -= Time.deltaTime;
        status = CheckLooseConditions();
    }

    private void DisplayMessage(string key)
    {
        string msg = FindByKey(key);
        uiManager.DisplayTextMessage(msg);
    }

    private void ProcessInteraction(MapTile currentTile, TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Carrot:
                tilemapHandler.ChangeTile(new Vector3Int(currentTile.Position.y, -currentTile.Position.x, 0), tileType);
                uiManager.UpdateScore(++carrotsPicked, carrotsAll, bonusesPicked);
                break;
            case TileType.Bonus:
                tilemapHandler.ChangeTile(new Vector3Int(currentTile.Position.y, -currentTile.Position.x, 0), tileType);
                uiManager.UpdateScore(carrotsPicked, carrotsAll, ++bonusesPicked);
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
                tilemapHandler.ChangeTile(new Vector3Int(currentTile.Position.y, -currentTile.Position.x, 0), tileType);

                break;
            default:
                break;
        }
    }

    private string FindByKey(string key)
    {
        captionHandler.dictionary.TryGetValue(key, out string value);
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
}
