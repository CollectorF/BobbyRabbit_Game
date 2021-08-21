using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameplayUI;
    [SerializeField]
    private GameObject messagePanel;
    [SerializeField]
    private float messageDisplayTime;
    [SerializeField]
    private TMP_Text carrotsText;
    [SerializeField]
    private TMP_Text bonusesText;

    private TMP_Text messagePanelText;
    private Coroutine timerCoroutine;

    private void Start()
    {
        messagePanelText = messagePanel.GetComponentInChildren<TMP_Text>();
        gameplayUI.SetActive(true);
        messagePanel.SetActive(false);
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
        carrotsText.text = $"{carrots.ToString()}/{carrotsAll.ToString()}";
        bonusesText.text = bonuses.ToString();
    }

    private IEnumerator DisplayByTimeCoroutine(GameObject obj, float time)
    {
        obj = messagePanel;
        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
