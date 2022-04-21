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
    internal Dictionary<string, string> dictionary;
    private string selectedLocale;

    internal Action OnLocaleDictFill;

    private void Awake()
    {
        dictionary = new Dictionary<string, string>();
        jsonReader = new JSONReader(dictionary);
        SetDefaultLocale(defaultLocale);
        mainMenu.OnLocaleButtonClick += SetLocale;
    }

    private void SetDefaultLocale(string locale)
    {
        foreach (var item in localizations)
        {
            if (item == locale)
            {
                selectedLocale = item;
            }
        }
        FillDictionary(selectedLocale);
    }

    private void SetLocale(string locale)
    {
        foreach (var item in localizations)
        {
            if (item.Contains(locale))
            {
                selectedLocale = item;
            }
        }
        FillDictionary(selectedLocale);
        OnLocaleDictFill?.Invoke();
    }

    private void FillDictionary(string locale)
    {
        string path = localeFolder + "/" + locale;
        dictionary = jsonReader.ReadJson(path);
    }
}
