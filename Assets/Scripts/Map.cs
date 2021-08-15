using UnityEngine;

public enum TileType
{
    Empty,
    Road,
    Carrot
}
public struct MapTile
{
    public TileType Type;

    public MapTile(TileType tileType)
    {
        Type = tileType;
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
}
