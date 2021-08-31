using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationHandler : MonoBehaviour
{
    [SerializeField]
    private string localization;

    private JSONReader jsonReader;
    internal Dictionary<string, string> dictionary = new Dictionary<string, string>();

    private void Awake()
    {
        jsonReader = new JSONReader(dictionary);
        FillDictionary(localization);
    }

    private void FillDictionary(string levelName)
    {
        dictionary = jsonReader.ReadJson(levelName);
    }
}
