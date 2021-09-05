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

[ExecuteInEditMode]
public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private string levelDir = "Level\\Level";
    [SerializeField]
    private List<Tiles> tiles;

    private ILevelLoader levelLoader;
    private ILevelFiller levelFiller;
    private string backgroundPostfix = "_Background";
    internal Map map;

    internal event Action OnLevelLoad;

    private void Awake()
    {
        Dictionary<char, Tile> tileAssets = new Dictionary<char, Tile>();
        Dictionary<char, TileType> loadingResource = new Dictionary<char, TileType>();
        foreach (var tile in tiles)
        {
            tileAssets.Add(tile.Code, tile.TileAsset);
            loadingResource.Add(tile.Code, tile.Type);
        }
        levelLoader = new ResourcesLevelLoader(tileAssets, loadingResource);
        levelFiller = new TilemapLevelFiller(tilemap, tileAssets);
    }

    internal void SetupLevel(int levelid)
    {
        string levelName = null;
        if (tag == "MainLoader")
        {
            levelName = levelDir + (levelid + 1);
        }
        if (tag == "BackgroundLoader")
        {
            levelName = levelDir + (levelid + 1) + backgroundPostfix;
        }
        tilemap.ClearAllTiles();
        map = levelLoader.ReadLevel(levelName);
        levelFiller.FillLevel(map);
        OnLevelLoad?.Invoke();
    }
}

