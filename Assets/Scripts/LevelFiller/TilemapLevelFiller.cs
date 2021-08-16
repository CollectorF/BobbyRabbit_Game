using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TilemapLevelFiller : ILevelFiller
{
    private Tilemap tilemap;
    //private Dictionary<MapTile, TileType> tileLibrary = new Dictionary<MapTile, TileType>();
    private Dictionary<MapTile, Tile> tileAssets = new Dictionary<MapTile, Tile>();

    //public TilemapLevelFiller(Tilemap tilemap, Dictionary<MapTile, TileType> tileLibrary, Dictionary<Tile, MapTile> tileAssets)
    public TilemapLevelFiller(Tilemap tilemap, Dictionary<MapTile, Tile> tileAssets)
    {
        this.tilemap = tilemap;
        //this.tileLibrary = tileLibrary;
        this.tileAssets = tileAssets;
    }


    public void FillLevel(Map map)
    {
        for (int x = 0; x < map.GetMapSize().x; x++)
        {
            for (int y = 0; y < map.GetMapSize().y; y++)
            {
                foreach (KeyValuePair<MapTile, Tile> tile in tileAssets)
                {
                    //if (.Equals(tile.Value))
                    //{

                    //}
                    tileAssets.TryGetValue(map.GetTileAt(x, y), out Tile tileOut);
                    tilemap.SetTile(new Vector3Int(x, -y, 0), tileOut);
                }
            }
        }
    }
}
