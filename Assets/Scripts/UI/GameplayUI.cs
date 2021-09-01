using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private GameObject messagePanel;
    [SerializeField]
    private float messageDisplayTime;
    [SerializeField]
    private TMP_Text carrotsText;
    [SerializeField]
    private TMP_Text bonusesText;
    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private float highlightOnTimeLeft;

    private TMP_Text messagePanelText;
    private Coroutine timerCoroutine;
    private UIManager uiManager;

    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
        messagePanelText = messagePanel.GetComponentInChildren<TMP_Text>();
        messagePanel.SetActive(false);
        timerText.color = Color.white;
        uiManager.OnDisplayTextMessage += DisplayTextMessage;
        uiManager.OnUpdateScore += UpdateScore;
        uiManager.OnUpdateTimer += UpdateTimer;
    }

    internal void DisplayTextMessage(string msg)
    {
        messagePanelText.text = msg;
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(DisplayByTimeCoroutine(messagePanel, messageDisplayTime));
        }
        timerCoroutine = null;
    }

    public void UpdateScore(int carrots, int carrotsAll, int bonuses)
    {
        carrotsText.text = $"{carrots}/{carrotsAll}";
        bonusesText.text = bonuses.ToString();
    }

    public void UpdateTimer(string timer, float timeLeft)
    {
        if (highlightOnTimeLeft > timeLeft)
        {
            timerText.color = Color.red;
        }
        timerText.text = timer;
    }

    private IEnumerator DisplayByTimeCoroutine(GameObject obj, float time)
    {
        obj = messagePanel;
        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
