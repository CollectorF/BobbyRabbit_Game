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
                levels.Add(new Level(level.Name, level.Difficulty, level.Timer, level.IsLocked, level.Obstacles));
                i++;
            }
            catch (NullReferenceException)
            {
                break;
            }
        }
        return levels;
    }

    //public Level ReadLevelInfo(string levelId)
    //{
    //    var fullFileName = string.Concat(levelId, levelInfoFilePostfix);
    //    string json = Resources.Load(fullFileName).ToString();
    //    Level level = JsonConvert.DeserializeObject<Level>(json);
    //    return new Level(level.Name, level.Difficulty, level.Timer, level.IsLocked, level.Obstacles);
    //}
}