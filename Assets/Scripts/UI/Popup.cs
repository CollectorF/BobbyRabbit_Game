using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField]
    internal TextMeshProUGUI popupText;
    [SerializeField]
    internal TextMeshProUGUI popupYes;
    [SerializeField]
    internal TextMeshProUGUI popupNo;

    [Space(20)]
    [SerializeField]
    internal string POPUP_WARNING_KEY = "PopupWarning";
    [SerializeField]
    internal string POPUP_YES_KEY = "Yes";
    [SerializeField]
    internal string POPUP_NO_KEY = "No";

    public void UpdatePopup(string warning, string yes, string no)
    {
        popupText.text = warning;
        popupYes.text = yes;
        popupNo.text = no;
    }

    public void HidePopup()
    {
        enabled = false;
    }

    //public void OnPopupYes()
    //{
    //    if (popupCaller == "LevelMenu")
    //    {
    //        OnClearPrefs?.Invoke();
    //        levelMenuManager.UpdateLevelList();
    //        HidePopup();
    //    }
    //    if (popupCaller == "GameplayUI")
    //    {
    //        ActivateLevelMenu();
    //        HidePopup();
    //    }
    //}

    public void OnPopupNo()
    {
        HidePopup();
    }
}
