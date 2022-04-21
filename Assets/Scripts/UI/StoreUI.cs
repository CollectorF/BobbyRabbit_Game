using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    [SerializeField]
    internal TextMeshProUGUI mainText;
    [SerializeField]
    internal TextMeshProUGUI bonuses;
    [SerializeField]
    internal TextMeshProUGUI quitButton;

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


