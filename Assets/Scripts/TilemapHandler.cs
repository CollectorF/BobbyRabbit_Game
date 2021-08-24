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
    }
}
