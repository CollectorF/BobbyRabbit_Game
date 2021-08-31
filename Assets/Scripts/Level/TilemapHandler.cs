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

    private Tilemap tilemapMain;
    private LevelLoader levelLoaderMain;
    private MapTile tileToChange;

    private void Start()
    {
        tilemapMain = GetComponent<Tilemap>();
        levelLoaderMain = GetComponent<LevelLoader>();
        SetObstacleInitialState(levelLoaderMain.map, levelLoaderMain.level);
    }

    private void SetObstacleInitialState(Map map, Level level)
    {
        for (int i = 0; i < map.Obstacles.Count; i++)
        {
            Vector3Int position = new Vector3Int(map.Obstacles[i].Position.x, -map.Obstacles[i].Position.y, 0);
            levelLoaderMain.map.SetTileState(position, level.obstacles[i]);
        }
    }

    internal void ChangeTile(Vector3Int position, TileType tileType)
    {
        if (tileType == TileType.Carrot)
        {
            tilemapMain.SetTile(position, carrotEmpty);
            levelLoaderMain.map.SetTileType(position, TileType.Background);
        }
        if (tileType == TileType.Bonus)
        {
            tilemapMain.SetTile(position, null);
            levelLoaderMain.map.SetTileType(position, TileType.Background);
        }
        if (tileType == TileType.ButtonOnOff)
        {
            if (levelLoaderMain.map.GetTileAt(position.x, -position.y).IsOn)
            {
                tilemapMain.SetTile(position, buttonOff);
                levelLoaderMain.map.SetTileState(position, false);
            }
            else
            {
                tilemapMain.SetTile(position, buttonOn);
                levelLoaderMain.map.SetTileState(position, true);
            }
        }
        if (tileType == TileType.InteractiveObstacle)
        {
            tileToChange = levelLoaderMain.map.GetTileAt(position.x, -position.y);
            if (tileToChange.Tile == spikesOn)
            {
                tilemapMain.SetTile(position, spikesOff);
                levelLoaderMain.map.SetTileState(position, false);
                levelLoaderMain.map.SetTileType(position, TileType.Walkable);
            }
            if (tileToChange.Tile == spikesOff)
            {
                tilemapMain.SetTile(position, spikesOn);
                levelLoaderMain.map.SetTileState(position, false);
                levelLoaderMain.map.SetTileType(position, TileType.InteractiveObstacle);
            }
        }
    }
}
