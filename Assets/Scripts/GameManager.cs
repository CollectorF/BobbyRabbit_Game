using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    private CaptionHandler captionHandler;
    private PlayerController playerController;
    private InteractionHandler interactionProcessor;
    private int carrotsAll;
    private int carrotsPicked = 0;
    private int bonusesPicked = 0;
    private float secondsLeft;
    private float secondsToPassLevel;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        interactionProcessor = player.GetComponent<InteractionHandler>();
        captionHandler = GetComponent<CaptionHandler>();

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
}
