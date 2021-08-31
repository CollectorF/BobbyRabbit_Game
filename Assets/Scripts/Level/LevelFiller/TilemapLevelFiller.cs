using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TilemapLevelFiller : ILevelFiller
{
    private Tilemap tilemap;
    private Dictionary<char, Tile> tileAssets = new Dictionary<char, Tile>();

    public TilemapLevelFiller(Tilemap tilemap, Dictionary<char, Tile> tileAssets)
    {
        this.tilemap = tilemap;
        this.tileAssets = tileAssets;
    }


    public void FillLevel(Map map)
    {
        for (int x = 0; x < map.GetMapSize().x; x++)
        {
            for (int y = 0; y < map.GetMapSize().y; y++)
            {
                foreach (KeyValuePair<char, Tile> tile in tileAssets)
                {
                    tileAssets.TryGetValue(map.GetTileAt(x, y).Code, out Tile tileOut);
                    tilemap.SetTile(new Vector3Int(x, -y, 0), tileOut);
                }
            }
        }
    }
}
