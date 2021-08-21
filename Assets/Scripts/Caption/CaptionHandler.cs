using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptionHandler : MonoBehaviour
{
    [SerializeField]
    private string levelName;

    private JSONReader jsonReader;
    internal Dictionary<string, string> dictionary = new Dictionary<string, string>();

    private void Awake()
    {
        jsonReader = new JSONReader(dictionary);
        FillDictionary(levelName);
    }

    private void FillDictionary(string levelName)
    {
        dictionary = jsonReader.ReadJson(levelName);
    }
}
