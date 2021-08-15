using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TilemapLevelFiller : ILevelFiller
{
    private Tilemap tilemap;
    private Dictionary<TileType, MapTile> tileLibrary = new Dictionary<TileType, MapTile>();

    public TilemapLevelFiller(Tilemap tilemap, Dictionary<TileType, MapTile> tileLibrary)
    {
        this.tilemap = tilemap;
        this.tileLibrary = tileLibrary;
    }


    public void FillLevel(Map map)
    {
        for (int y = 0; y < map.GetMapSize().y; y++)
        {
            for (int x = 0; x < map.GetMapSize().x; x++)
            {
                foreach (KeyValuePair<TileType, MapTile> tile in tileLibrary)
                {
                    if (map.GetTileAt(x, y).Equals(tile.Value))
                    {
                        tilemap.SetTile(new Vector3Int(x, -y, 0), (Tile)tile.Key);
                    }
                }
            }
        }
    }
}
