﻿using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Empty,
    Road,
    Tree,
    Carrot
}
public struct MapTile
{
    public TileType Type;
    public Tile Tile;

    public MapTile(TileType tileType, Tile tile)
    {
        Type = tileType;
        Tile = tile;
    }
}

public class Map
{
    private MapTile[][] map;

    public MapTile GetTileAt(int x, int y)
    {
        return map[x][y];
    }

    public Vector2Int GetMapSize()
    {
        if (map.Length == 0)
        {
            return new Vector2Int(0, 0);
        }
        return new Vector2Int(map.Length, map[0].Length);
    }

    public Map(MapTile[][] tiles)
    {
        map = tiles;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj);
    }
}
