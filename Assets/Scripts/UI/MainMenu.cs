using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    internal Action<string> OnLocaleButtonClick;

    public void OnLocaleClick(string locale)
    {
        OnLocaleButtonClick?.Invoke(locale);
    }
}
