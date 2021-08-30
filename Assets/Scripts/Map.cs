using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Background,
    Obstacle,
    Carrot,
    Bonus,
    StartPoint,
    Default,
    Walkable,
    FinishPoint,
    ButtonOnOff,
    Switch,
}
public struct MapTile
{
    public TileType Type;
    public Tile Tile;
    public char Code;
    public Vector2Int Position;
    public bool IsOn;

    public MapTile(TileType tileType, Tile tile, char code, Vector2Int position, bool isOn)
    {
        Type = tileType;
        Tile = tile;
        Code = code;
        Position = position;
        IsOn = isOn;
    }
}

public class Map
{
    private MapTile[][] map;
    public int CarrotQuantity { get; private set; }
    public int BonusQuantity { get; private set; }
    public List<MapTile> Buttons = new List<MapTile>();
    public List<MapTile> Obstacles = new List<MapTile>();

    public Map(MapTile[][] tiles, int carrots, int bonuses, List<MapTile> buttons, List<MapTile> obstacles)
    {
        map = tiles;
        CarrotQuantity = carrots;
        BonusQuantity = bonuses;
        Buttons = buttons;
        Obstacles = obstacles;
    }

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

    public void SetTileType(Vector3Int position, TileType tileType)
    {
        map[-position.y][position.x].Type = tileType;
    }

    public void SetTileState(Vector3Int position, bool state)
    {
        map[-position.y][position.x].IsOn = state;
    }
}
