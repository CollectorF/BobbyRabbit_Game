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
    private GameObject player;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private string levelName;
    [SerializeField]
    [Tooltip("Set at least for one Level Loader on the scene")]
    private bool loadLevelInfo;
    [SerializeField]
    private List<Tiles> tiles;

    private ILevelLoader levelLoader;
    private ILevelInfoLoader levelInfoLoader;
    private ILevelFiller levelFiller;
    internal Map map;
    internal Level level;

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
        levelInfoLoader = new ResourcesLevelInfoLoader();
        levelFiller = new TilemapLevelFiller(tilemap, tileAssets);
        SetupLevel(levelName);
    }

    private void SetupLevel(string levelName)
    {
        tilemap.ClearAllTiles();
        tilemap.size = new Vector3Int(50, 50, 0);
        map = levelLoader.ReadLevel(levelName);
        levelFiller.FillLevel(map);
        if (player != null)
        {
            MapTile startTile = map.GetSingleTileByType(TileType.StartPoint);
            Vector2 playerStartPoint = map.GetTileCenter(tilemap, startTile);
            player.transform.position = playerStartPoint;
        }
        if (loadLevelInfo)
        {
            level = levelInfoLoader.ReadLevelInfo(levelName);
        }
    }
}

