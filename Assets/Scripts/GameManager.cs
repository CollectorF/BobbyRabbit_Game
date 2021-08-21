using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        interactionProcessor = player.GetComponent<InteractionHandler>();
        captionHandler = GetComponent<CaptionHandler>();

        playerController.OnNoWay += DisplayMessage;
        interactionProcessor.OnInteraction += ProcessInteraction;
        carrotsAll = levelLoaderMain.map.carrotQuantity;
        uiManager.UpdateScore(carrotsPicked, carrotsAll, bonusesPicked);
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
}
