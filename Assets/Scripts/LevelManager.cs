using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct Tiles
{
    public char Code;
    public TileType Type;
    public MapTile MapTile;
    public Tile Tile;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private List<Tiles> tiles;

    private ILevelLoader levelLoader;
    private ILevelFiller levelFiller;

    private void Awake()
    {
        Dictionary<char, TileType> loadingResource = new Dictionary<char, TileType>();
        Dictionary<TileType, MapTile> tileLibrary = new Dictionary<TileType, MapTile>();
        foreach (var tile in tiles)
        {
            loadingResource.Add(tile.Code, tile.Type);
            tileLibrary.Add(tile.Type, tile.MapTile);
        }
        levelLoader = new ResourcesLevelLoader(loadingResource);
        levelFiller = new TilemapLevelFiller(tilemap, tileLibrary);

        SetupLevel("Level1");
    }

    private void SetupLevel(string levelName)
    {
        tilemap.ClearAllTiles();
        Map map = levelLoader.ReadLevel(levelName);
        levelFiller.FillLevel(map);
    }
}

