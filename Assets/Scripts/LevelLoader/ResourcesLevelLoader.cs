using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourcesLevelLoader : ILevelLoader
{
    private Dictionary<char, Tile> tileLibrary;
    private Dictionary<char, TileType> tileTypeLibrary;

    public ResourcesLevelLoader(Dictionary<char, Tile> tileLibrary, Dictionary<char, TileType> tileTypeLibrary)
    {
        this.tileLibrary = tileLibrary;
        this.tileTypeLibrary = tileTypeLibrary;
    }

    public Map ReadLevel(string levelId)
    {
        string text = Resources.Load(levelId).ToString();
        string[] lines = Regex.Split(text, "\r\n");
        int rows = lines.Length;

        MapTile[][] levelBase = new MapTile[rows][];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] castedCode = lines[i].Split(' ');
            levelBase[i] = new MapTile[castedCode.Length];
            for (int j = 0; j < levelBase[i].Length; j++)
            {
                char code = castedCode[j][0];
                Vector2Int position = new Vector2Int(i, j);
                tileLibrary.TryGetValue(castedCode[j][0], out Tile tile);
                tileTypeLibrary.TryGetValue(castedCode[j][0], out TileType type);
                levelBase[i][j] = new MapTile(type, tile, code, position);
            }
        }
        return new Map(levelBase);
    }
}