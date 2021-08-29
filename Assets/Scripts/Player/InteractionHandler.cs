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
        lastTile = playerController.GetCurrentTile(mainLevelLoader, playerController.currentPositionVector);
    }

    void Update()
    {
        currentTile = playerController.GetCurrentTile(mainLevelLoader, playerController.currentPositionVector);
        if (currentTile.Position != lastTile.Position)
        {
            CheckInteraction(currentTile, TileType.Carrot);
            CheckInteraction(currentTile, TileType.Bonus);
            CheckInteraction(currentTile, TileType.FinishPoint);
            CheckInteraction(currentTile, TileType.ButtonOnOff);
        }
        lastTile = currentTile;
    }

    private void CheckInteraction(MapTile currentTile, TileType tileType)
    {
        if (currentTile.Type == tileType)
        {
            OnInteraction?.Invoke(currentTile, tileType);
        }
    }
}
