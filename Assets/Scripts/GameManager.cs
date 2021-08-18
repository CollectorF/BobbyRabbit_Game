using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private UIManager uiManager;

    private void Start()
    {
        playerController.OnNoWay += DisplayMessage;
    }

    private void DisplayMessage(string msg)
    {
        uiManager.DisplayTextMessage(msg);
    }
}
