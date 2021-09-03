using System.Collections.Generic;

interface ILevelInfoLoader
{
    //Level ReadLevelInfo(string levelId);

    List<Level> ReadAllLevelsInfo(string levelId);
}
