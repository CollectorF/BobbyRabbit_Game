using Newtonsoft.Json;
using UnityEngine;

public class ResourcesLevelInfoLoader : ILevelInfoLoader
{
    private string levelInfoFileSuffix = "_Info";

    public Level ReadLevelInfo(string levelId)
    {
        var fullFileName = string.Concat(levelId, levelInfoFileSuffix);
        string json = Resources.Load(fullFileName).ToString();
        Level level = JsonConvert.DeserializeObject<Level>(json);
        return new Level(level.name, level.difficulty, level.timer);
    }
}