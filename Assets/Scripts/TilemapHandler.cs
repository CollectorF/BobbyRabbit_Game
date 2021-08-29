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

    private void Start()
    {
        tilemapMain = GetComponent<Tilemap>();
        levelLoaderMain = GetComponent<LevelLoader>();
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
    }
}
