using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private string BONUS = "Bonus";
    private string UNLOCKED_LEVELS = "Unlocked_Levels";

    public List<Level> LoadPlayerPrefs(List<Level> levels, out int bonuses)
    {
        bonuses = 0;
        int unlockedLevels = PlayerPrefs.GetInt(UNLOCKED_LEVELS, 1);
        PlayerPrefs.GetInt(BONUS, 0);
        for (int i = 0; i < unlockedLevels; i++)
        {
            levels[i].IsOpen = true;
        }
        return levels;
    }

    public void SavePlayerPrefs(int bonuses, int unlockedLevels)
    {
        PlayerPrefs.SetInt(BONUS, bonuses);
        PlayerPrefs.SetInt(UNLOCKED_LEVELS, unlockedLevels);
        PlayerPrefs.Save();
    }

    public List<Level> ClearPlayerPrefs(List<Level> levels, out int bonuses)
    {
        PlayerPrefs.DeleteAll();
        bonuses = PlayerPrefs.GetInt(BONUS, 0);
        levels[0].IsOpen = true;
        for (int i = 1; i < levels.Count; i++)
        {
            levels[i].IsOpen = false;
        }
        return levels;
    }
}
