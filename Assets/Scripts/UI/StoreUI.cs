using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    [SerializeField]
    internal TMP_Text mainText;
    [SerializeField]
    internal TMP_Text bonuses;
    [SerializeField]
    internal TMP_Text quitButton;

    [Space(20)]
    [SerializeField]
    internal string STORE_TEXT_KEY = "StoreText";

    public void UpdateMenu(string main, string quit)
    {
        mainText.text = main;
        quitButton.text = quit;
    }

    public void UpdateBonuses(int bonus)
    {
        bonuses.text = bonus.ToString();
    }
}


