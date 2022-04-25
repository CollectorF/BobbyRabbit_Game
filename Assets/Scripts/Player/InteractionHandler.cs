using System;
using UnityEngine;

// This class is checking in Update, is player moved to new tile from old one, and if he did - 
// checking is new tile is interactible. If it is - invoking event, on which the GameManager is subscribed.
// All the actions on interaction are processed in GameManager.

public class InteractionHandler : MonoBehaviour
{
    [SerializeField]
    private LevelLoader levelLoaderMain;

    private PlayerController playerController;
    private MapTile currentTile;
    private MapTile lastTile;
    private int number;

    internal event Action<MapTile, TileType, int?> OnInteraction;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (levelLoaderMain.map != null)
        {
            lastTile = playerController.GetCurrentTile(levelLoaderMain, playerController.currentPositionVector);
        }
    }

    void Update()
    {
        if (levelLoaderMain.map != null)
        {
            currentTile = playerController.GetCurrentTile(levelLoaderMain, playerController.currentPositionVector);
            foreach (var button in levelLoaderMain.map.Buttons)
            {
                if (currentTile.Position == button.Position)
                {
                    number = levelLoaderMain.map.Buttons.IndexOf(button);
                }
            }
            if (currentTile.Position != lastTile.Position)
            {
                CheckInteraction(currentTile, TileType.Carrot, null);
                CheckInteraction(currentTile, TileType.Bonus, null);
                CheckInteraction(currentTile, TileType.FinishPoint, null);
                CheckInteraction(currentTile, TileType.ButtonOnOff, number);
            }
            lastTile = currentTile;
        }
    }

    private void CheckInteraction(MapTile currentTile, TileType tileType, int? number)
    {
        if (currentTile.Type == tileType)
        {
            OnInteraction?.Invoke(currentTile, tileType, number);
        }
    }
}
