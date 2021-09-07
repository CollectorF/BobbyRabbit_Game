using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[Serializable]
//public struct ReplacementTiles
//{
//    public string Name;
//    public Tile Tile;

//    public ReplacementTiles(string name, Tile tile)
//    {
//        Name = name;
//        Tile = tile;
//    }
//}

public class TilemapHandler : MonoBehaviour
{
    //[SerializeField]
    //private List<ReplacementTiles> replacementTiles;
    [SerializeField]
    private Tile carrotEmpty;
    [SerializeField]
    private Tile buttonOn;
    [SerializeField]
    private Tile buttonOff;
    [SerializeField]
    private Tile spikesOn;
    [SerializeField]
    private Tile spikesOff;

    private LevelInfoHandler levelInfoHandler;
    private Tilemap tilemapMain;
    private LevelLoader levelLoaderMain;
    private MapTile tileToChange;

    private void Start()
    {
        tilemapMain = GetComponent<Tilemap>();
        levelLoaderMain = GetComponent<LevelLoader>();
        levelInfoHandler = GetComponentInParent<LevelInfoHandler>();
    }

    internal void SetObstacleInitialState(Map map, List<Level> levels, int levelId)
    {
        for (int i = 0; i < map.Obstacles.Count; i++)
        {
            Vector3Int position = new Vector3Int(map.Obstacles[i].Position.x, -map.Obstacles[i].Position.y, 0);
            levelLoaderMain.map.SetTileState(position, levels[levelId].Obstacles[i]);
        }
    }

    internal bool ChangeTile(Vector3Int position, TileType tileType)
    {
        Vector3Int newPosition = new Vector3Int(-position.y, -position.x, 0);
        bool state = false;
        if (tileType == TileType.Carrot)
        {
            tilemapMain.SetTile(position, carrotEmpty);
            levelLoaderMain.map.SetTileType(newPosition, TileType.Background);
        }
        if (tileType == TileType.Bonus)
        {
            tilemapMain.SetTile(position, null);
            levelLoaderMain.map.SetTileType(newPosition, TileType.Background);
        }
        if (tileType == TileType.ButtonOnOff)
        {
            
            var currentButton = levelLoaderMain.map.GetTileAt(position.x, -position.y);
            if (currentButton.IsOn)
            {
                tilemapMain.SetTile(position, buttonOff);
                levelLoaderMain.map.SetTileState(newPosition, false);
            }
            else
            {
                tilemapMain.SetTile(position, buttonOn);
                levelLoaderMain.map.SetTileState(newPosition, true);
                state = true;
            }
        }
        return state;
    }
    internal void ChangeInteractiveObstacle(bool controlButtonState, int i)
    {
        Vector3Int position = new Vector3Int(levelLoaderMain.map.Obstacles[i].Position.x, -levelLoaderMain.map.Obstacles[i].Position.y, 0); //input Xpos, Yneg
        tileToChange = levelLoaderMain.map.GetTileAt(-position.y, position.x); //ONLY positive Y position (-/- = +) !!!
        if (tileToChange.Tile == spikesOn || tileToChange.Tile == spikesOff)
        {
            if (controlButtonState)
            {
                tilemapMain.SetTile(new Vector3Int(levelLoaderMain.map.Obstacles[i].Position.y, -levelLoaderMain.map.Obstacles[i].Position.x, 0), spikesOff);
                levelLoaderMain.map.SetTileState(position, false); //ONLY negative Y position !!!
                levelLoaderMain.map.SetTileType(position, TileType.Walkable);
            }
            else
            {
                tilemapMain.SetTile(new Vector3Int(levelLoaderMain.map.Obstacles[i].Position.y, -levelLoaderMain.map.Obstacles[i].Position.x, 0), spikesOn);
                levelLoaderMain.map.SetTileState(position, true);
                levelLoaderMain.map.SetTileType(position, TileType.InteractiveObstacle);
            }
        }
    }
}
