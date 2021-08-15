using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ResourcesLevelLoader : ILevelLoader
{
    private Dictionary<char, TileType> tileLibrary;

    public ResourcesLevelLoader(Dictionary<char, TileType> tileLibrary)
    {
        this.tileLibrary = tileLibrary;
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
                tileLibrary.TryGetValue(stringsOfLine[j][0], out TileType type);
                levelBase[i][j] = new MapTile(type);
            }
        }
        return new Map(levelBase);
    }
}