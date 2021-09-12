using System;
using System.Collections;
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
    [SerializeField]
    internal TMP_Text quitButton;

    [Space(20)]
    [SerializeField]
    internal string QUIT_KEY = "Quit";

    private TMP_Text messagePanelText;
    private Coroutine timerCoroutine;
    private UIManager uiManager;
    internal event Action<string> OnQuitButtonClick;

    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
        messagePanelText = messagePanel.GetComponentInChildren<TMP_Text>();
        messagePanel.SetActive(false);
        timerText.color = Color.white;
        uiManager.OnDisplayTextMessage += DisplayTextMessage;
        uiManager.OnUpdateTimer += UpdateTimer;
    }

    internal void DisplayTextMessage(string msg)
    {
        if (isActiveAndEnabled && !messagePanel.activeSelf)
        {
            messagePanelText.text = msg;
            if (timerCoroutine == null)
            {
                timerCoroutine = StartCoroutine(DisplayByTimeCoroutine(messagePanel, messageDisplayTime));
            }
            timerCoroutine = null;
        }
    }

    public void UpdateScore(int carrots, int carrotsAll, int bonuses)
    {
        carrotsText.text = $"{carrots}/{carrotsAll}";
        bonusesText.text = bonuses.ToString();
    }

    public void UpdateTimer(string timer, float timeLeft)
    {
        timerText.color = Color.white;
        if (highlightOnTimeLeft > timeLeft)
        {
            timerText.color = Color.red;
        }
        timerText.text = timer;
    }

    public void UpdateMenu(string quit)
    {
        quitButton.text = quit;
    }

    public void OnQuitClick()
    {
        OnQuitButtonClick?.Invoke(tag);
    }

    private IEnumerator DisplayByTimeCoroutine(GameObject obj, float time)
    {
        obj = messagePanel;
        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
