using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ResourcesLevelInfoLoader : ILevelInfoLoader
{
    private string levelInfoFilePostfix = "_Info";
    private int i = 1;

    public List<Level> ReadAllLevelsInfo(string levelFile)
    {
        string json = null;
        List<Level> levels = new List<Level>();
        while (true)
        {
            var fullFileName = $"{levelFile}{i}{levelInfoFilePostfix}";
            try
            {
                json = Resources.Load(fullFileName).ToString();
                Level level = JsonConvert.DeserializeObject<Level>(json);
                levels.Add(new Level(level.DifficultyString, level.Timer, level.IsOpen, level.Obstacles));
                i++;
            }
            catch (NullReferenceException)
            {
                break;
            }
        }
        return levels;
    }
}