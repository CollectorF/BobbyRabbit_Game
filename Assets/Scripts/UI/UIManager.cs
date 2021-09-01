using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject levelMenu;
    [SerializeField]
    private GameObject gameplayUI;

    internal event Action<string> OnDisplayTextMessage;
    internal event Action<int, int, int> OnUpdateScore;
    internal event Action<string, float> OnUpdateTimer;

    private void Start()
    {
        mainMenu.SetActive(true);
        //levelMenu.SetActive(false);
        gameplayUI.SetActive(false);
    }

    internal void DisplayMessage(string msg)
    {
        OnDisplayTextMessage?.Invoke(msg);
    }

    public void ScoreUpdate(int carrots, int carrotsAll, int bonuses)
    {
        OnUpdateScore?.Invoke(carrots, carrotsAll, bonuses);
    }
    public void TimerUpdate(string timer, float timeLeft)
    {
        OnUpdateTimer?.Invoke(timer, timeLeft);
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        //levelMenu.SetActive(false);
        gameplayUI.SetActive(true);
    }
}
