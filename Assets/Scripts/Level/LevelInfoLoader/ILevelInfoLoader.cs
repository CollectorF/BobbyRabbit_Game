using System.Collections.Generic;

interface ILevelInfoLoader
{
    List<Level> ReadAllLevelsInfo(string levelId);
}
