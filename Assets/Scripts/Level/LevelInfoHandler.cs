using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfoHandler : MonoBehaviour
{
    [SerializeField]
    private string levelsPath = "Level\\Level";

    private ILevelInfoLoader levelInfoLoader;
    internal Level level;
    internal List<Level> levels;

    private void Awake()
    {
        levelInfoLoader = new ResourcesLevelInfoLoader();
        levels = levelInfoLoader.ReadAllLevelsInfo(levelsPath);
    }
}
