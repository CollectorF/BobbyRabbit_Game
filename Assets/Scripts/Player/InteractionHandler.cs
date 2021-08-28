using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemapMain;
    [SerializeField]
    private LevelLoader mainLevelLoader;

    private PlayerController playerController;
    private MapTile currentTile;
    private MapTile lastTile;

    internal event Action<MapTile, TileType> OnInteraction;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        lastTile = GetCurrentTile(mainLevelLoader, tilemapMain);
    }

    void Update()
    {
        currentTile = GetCurrentTile(mainLevelLoader, tilemapMain);
        if (currentTile.Position != lastTile.Position)
        {
            CheckInteraction(currentTile, TileType.Carrot);
            CheckInteraction(currentTile, TileType.Bonus);
            CheckInteraction(currentTile, TileType.FinishPoint);
            CheckInteraction(currentTile, TileType.ButtonOnOff);
        }
        lastTile = currentTile;
    }

    private MapTile GetCurrentTile(LevelLoader levelLoader, Tilemap tilemap)
    {
        currentTile = levelLoader.map.GetTileAt(Mathf.FloorToInt(playerController.currentPositionVector.x), Mathf.FloorToInt(playerController.currentPositionVector.y));
        return currentTile;
    }

    private void CheckInteraction(MapTile currentTile, TileType tileType)
    {
        if (currentTile.Type == tileType)
        {
            OnInteraction?.Invoke(currentTile, tileType);
        }
    }
}
