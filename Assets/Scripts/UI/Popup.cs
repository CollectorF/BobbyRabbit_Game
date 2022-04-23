using System;
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

    private GameState currentState;

    internal event Action OnActivateLevelMenu;
    internal event Action OnClearPrefs;
    internal event Action OnUpdateLevelList;

    public void UpdatePopup(string warning, string yes, string no)
    {
        popupText.text = warning;
        popupYes.text = yes;
        popupNo.text = no;
    }

    public void ShowPopup()
    {
        gameObject.SetActive(true);
    }
    public void HidePopup()
    {
        gameObject.SetActive(false);
    }

    public void HandlePopup(GameState state)
    {
        ShowPopup();
        currentState = state;
    }

    public void OnPopupYes()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                OnActivateLevelMenu?.Invoke();
                HidePopup();
                break;
            case GameState.LevelMenu:
                OnClearPrefs?.Invoke();
                OnUpdateLevelList?.Invoke();
                HidePopup();
                break;
            default:
                break;
        }
    }
    public void OnPopupNo()
    {
        HidePopup();
    }
}
