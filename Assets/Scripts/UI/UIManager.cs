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

    private TMP_Text messagePanelText;
    private Coroutine timerCoroutine;

    private void Start()
    {
        messagePanelText = messagePanel.GetComponent<TMP_Text>();
        gameplayUI.SetActive(true);
        messagePanel.SetActive(false);
    }

    internal void DisplayTextMessage(string msg)
    {
        messagePanelText.text = msg;
        messagePanel.SetActive(true);
        timerCoroutine = StartCoroutine(TimerCoroutine(messageDisplayTime));
        messagePanel.SetActive(false);
        StopCoroutine(timerCoroutine);
    }

    private IEnumerator TimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
