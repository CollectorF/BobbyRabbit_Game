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
    private GameObject playerPrefab;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private string levelName;
    [SerializeField]
    private List<Tiles> tiles;

    private ILevelLoader levelLoader;
    private ILevelFiller levelFiller;
    internal Map map;
    internal GameObject player;

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

        SetupLevel(levelName);
    }

    private void SetupLevel(string levelName)
    {
        tilemap.ClearAllTiles();
        map = levelLoader.ReadLevel(levelName);
        levelFiller.FillLevel(map);
        if (playerPrefab != null)
        {
            MapTile startTile = map.GetSingleTileByType(TileType.PlayerStart);
            Vector2 playerSpawnPoint = map.GetTileCenter(tilemap, startTile);
            player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
            player.name = "Player";
        }
    }
}

