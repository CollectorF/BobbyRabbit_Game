using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Background,
    Obstacle,
    Carrot,
    Bonus,
    PlayerStart,
    Default,
    Walkable
}
public struct MapTile
{
    public TileType Type;
    public Tile Tile;
    public char Code;
    public Vector2Int Position;

    public MapTile(TileType tileType, Tile tile, char code, Vector2Int position)
    {
        Type = tileType;
        Tile = tile;
        Code = code;
        Position = position;
    }
}

public class Map
{
    private MapTile[][] map;

    public MapTile GetTileAt(int x, int y)
    {
        return map[y][x];
    }

    public MapTile GetSingleTileByType(TileType type)
    {
        MapTile foundedTile = map[0][0];
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                if (map[i][j].Type == type)
                {
                    foundedTile = map[i][j];
                    break;
                }
            }
        }
        return foundedTile;
    }

    public Vector2 GetTileCenter(Tilemap tilemap, MapTile tile)
    {
        Vector2 position = tilemap.GetCellCenterWorld(new Vector3Int(tile.Position.y, -tile.Position.x, 0));
        return position;
    }

    public Vector2Int GetMapSize()
    {
        if (map.Length == 0)
        {
            return Vector2Int.zero;
        }
        return new Vector2Int(map.Length, map[0].Length);
    }

    public Map(MapTile[][] tiles)
    {
        map = tiles;
    }
}
