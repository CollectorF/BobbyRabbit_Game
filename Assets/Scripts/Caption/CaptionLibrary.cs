using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptionLibrary : MonoBehaviour
{
    [SerializeField]
    private string levelName;

    private JSONReader jsonReader;
    internal Dictionary<string, string> captionDictionary = new Dictionary<string, string>();

    private void Awake()
    {
        jsonReader = new JSONReader(captionDictionary);
        FillDictionary(levelName);
    }

    private void FillDictionary(string levelName)
    {
        captionDictionary = jsonReader.ReadJson(levelName);
    }
}
