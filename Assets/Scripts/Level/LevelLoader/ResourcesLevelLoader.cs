using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourcesLevelLoader : ILevelLoader
{
    private Dictionary<char, Tile> tileLibrary;
    private Dictionary<char, TileType> tileTypeLibrary;
    private string levelInfoFilePostfix = "_Info";

    public ResourcesLevelLoader(Dictionary<char, Tile> tileLibrary, Dictionary<char, TileType> tileTypeLibrary)
    {
        this.tileLibrary = tileLibrary;
        this.tileTypeLibrary = tileTypeLibrary;
    }

    public Map ReadLevel(string levelId)
    {
        int carrots = 0;
        int bonuses = 0;
        List<MapTile> buttons = new List<MapTile>();
        List<MapTile> obstacles = new List<MapTile>();
        string text = Resources.Load(levelId).ToString();
        string[] lines = Regex.Split(text, "\r\n");
        MapTile[][] levelBase = new MapTile[lines.Length][];
        for (int i = 0; i <= lines.Length - 1; i++)
        {
            string[] castedCode = lines[i].Split(' ');
            levelBase[i] = new MapTile[castedCode.Length];
            for (int j = 0; j <= levelBase[i].Length - 1; j++)
            {
                bool isOn = false;
                char code = castedCode[j][0];
                Vector2Int position = new Vector2Int(i, j);
                tileLibrary.TryGetValue(castedCode[j][0], out Tile tile);
                tileTypeLibrary.TryGetValue(castedCode[j][0], out TileType type);
                levelBase[i][j] = new MapTile(type, tile, code, position, isOn);
                if (levelBase[i][j].Type == TileType.Carrot)
                {
                    carrots++;
                }
                if (levelBase[i][j].Type == TileType.Bonus)
                {
                    bonuses++;
                }
                if (levelBase[i][j].Type == TileType.ButtonOnOff)
                {
                    buttons.Add(levelBase[i][j]);
                }
                if (levelBase[i][j].Type == TileType.InteractiveObstacle)
                {
                    obstacles.Add(levelBase[i][j]);
                }
            }
        }
        return new Map(levelBase, carrots, bonuses, buttons, obstacles);
    }

    public Level ReadLevelInfo(string levelId)
    {
        var fullFileName = string.Concat(levelId, levelInfoFilePostfix);
        string json = Resources.Load(fullFileName).ToString();
        Level level = JsonConvert.DeserializeObject<Level>(json);
        return new Level(level.Name, level.Difficulty, level.Timer, level.IsLocked, level.Obstacles);
    }
}