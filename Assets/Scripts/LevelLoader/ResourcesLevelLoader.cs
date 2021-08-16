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
            string[] stringsOfLine = lines[i].Split(' ');
            levelBase[i] = new MapTile[stringsOfLine.Length];
            for (int j = 0; j < levelBase[i].Length; j++)
            {
                char code = stringsOfLine[j][0];
                tileLibrary.TryGetValue(stringsOfLine[j][0], out Tile tile);
                tileTypeLibrary.TryGetValue(stringsOfLine[j][0], out TileType type);
                levelBase[i][j] = new MapTile(type, tile, code);
            }
        }
        return new Map(levelBase);
    }
}