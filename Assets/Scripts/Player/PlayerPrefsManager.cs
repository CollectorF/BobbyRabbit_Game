using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private void Start()
    {
        //PlayerPrefs.GetInt(DEATH_KEY, 0)
    }
    private void OnDestroy()
    {
        //PlayerPrefs.SetInt(DEATH_KEY, deathCounter);
        //PlayerPrefs.SetInt(SPOT_KEY, spotCounter);
        PlayerPrefs.Save();
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
