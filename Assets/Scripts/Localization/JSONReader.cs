using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader
{
    private Dictionary<string, string> captionLibrary;

    public JSONReader(Dictionary<string, string> captionLibrary)
    {
        this.captionLibrary = captionLibrary;
    }

    public Dictionary<string, string> ReadJson(string filename)
    {
        string text = Resources.Load(filename).ToString();
        captionLibrary = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
        return captionLibrary;
    }
}
