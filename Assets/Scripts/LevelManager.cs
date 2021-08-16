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
    public Tile TileAsset;
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
        Dictionary<char, Tile> loadingResourceTiles = new Dictionary<char, Tile>();
        Dictionary<char, TileType> loadingResource = new Dictionary<char, TileType>();
        Dictionary<MapTile, Tile> tileAssets = new Dictionary<MapTile, Tile>();
        //Dictionary<MapTile, TileType> tileLibrary = new Dictionary<MapTile, TileType>();
        foreach (var tile in tiles)
        {
            loadingResourceTiles.Add(tile.Code, tile.TileAsset);
            loadingResource.Add(tile.Code, tile.Type);
            tileAssets.Add(tile.MapTile, tile.TileAsset);
            //tileLibrary.Add(tile.MapTile, tile.Type);
        }
        levelLoader = new ResourcesLevelLoader(loadingResourceTiles, loadingResource);
        levelFiller = new TilemapLevelFiller(tilemap, tileAssets);
        //levelFiller = new TilemapLevelFiller(tilemap, tileLibrary, tileAssets);

        SetupLevel("Level1");
    }

    private void SetupLevel(string levelName)
    {
        tilemap.ClearAllTiles();
        Map map = levelLoader.ReadLevel(levelName);
        levelFiller.FillLevel(map);
    }
}

