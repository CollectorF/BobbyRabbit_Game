using Newtonsoft.Json;
using UnityEngine;

public class ResourcesLevelInfoLoader : ILevelInfoLoader
{
    private string levelInfoFilePostfix = "_Info";

    public Level ReadLevelInfo(string levelId)
    {
        var fullFileName = string.Concat(levelId, levelInfoFilePostfix);
        string json = Resources.Load(fullFileName).ToString();
        Level level = JsonConvert.DeserializeObject<Level>(json);
        return new Level(level.name, level.difficulty, level.timer, level.obstacles);
    }
}