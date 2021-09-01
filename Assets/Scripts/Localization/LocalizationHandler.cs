using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationHandler : MonoBehaviour
{
    [SerializeField]
    private List<string> localizations = new List<string>();
    [SerializeField]
    private string defaultLocale;
    [SerializeField]
    private string localeFolder = "Localization";
    [SerializeField]
    private MainMenu mainMenu;

    private JSONReader jsonReader;
    internal Dictionary<string, string> dictionary = new Dictionary<string, string>();

    private void Awake()
    {
        jsonReader = new JSONReader(dictionary);
        mainMenu.OnLocaleButtonClick += FillDictionary;
    }

    private string SetLocale(string locale)
    {
        string selectedLocale = null;
        foreach (var item in localizations)
        {
            if (item.Contains(locale))
            {
                selectedLocale = item;
            }
            else
            {
                if (item == defaultLocale)
                {
                    selectedLocale = item;
                }
            }
        }
        return selectedLocale;
    }

    private void FillDictionary(string locale)
    {
        string selectedLocale = SetLocale(locale);
        string path = localeFolder + "/" + selectedLocale;
        dictionary = jsonReader.ReadJson(path);
    }
}
