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
        bool state = false;
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
                state = true;
            }
        }
        return state;
    }
    internal void ChangeInteractiveObstacle(bool controlButtonState, int i)
    {
        Vector3Int position = new Vector3Int(levelLoaderMain.map.Obstacles[i].Position.x, -levelLoaderMain.map.Obstacles[i].Position.y, 0);
        tileToChange = levelLoaderMain.map.GetTileAt(position.x, -position.y); //ONLY positive Y position (-/- = +) !!!
        if (tileToChange.Tile == spikesOn || tileToChange.Tile == spikesOff)
        {
            if (controlButtonState)
            {
                tilemapMain.SetTile(position, spikesOff);
                levelLoaderMain.map.SetTileState(position, false); //ONLY negative Y position !!!
                levelLoaderMain.map.SetTileType(position, TileType.Walkable);
            }
            else
            {
                tilemapMain.SetTile(position, spikesOn);
                levelLoaderMain.map.SetTileState(position, true);
                levelLoaderMain.map.SetTileType(position, TileType.InteractiveObstacle);
            }
        }
    }
}
