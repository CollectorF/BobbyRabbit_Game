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
    private MapTile currentPosition;

    internal event Action<MapTile, TileType> OnInteraction;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        currentPosition = GetCurrentTile(mainLevelLoader, tilemapMain);
        CheckInteraction(currentPosition, TileType.Carrot);
        CheckInteraction(currentPosition, TileType.Bonus);
        CheckInteraction(currentPosition, TileType.FinishPoint);
    }

    private MapTile GetCurrentTile(LevelLoader levelLoader, Tilemap tilemap)
    {
        currentPosition = levelLoader.map.GetTileAt(Mathf.FloorToInt(playerController.currentPositionVector.x), Mathf.FloorToInt(playerController.currentPositionVector.y));
        return currentPosition;
    }

    private void CheckInteraction(MapTile currentTile, TileType tileType)
    {
        if (currentTile.Type == tileType)
        {
            OnInteraction?.Invoke(currentTile, tileType);
        }
    }
}
