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
    private TextMeshProUGUI carrotsText;
    [SerializeField]
    private TextMeshProUGUI bonusesText;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private float highlightOnTimeLeft;
    [SerializeField]
    internal TextMeshProUGUI quitButton;
    [SerializeField]
    internal RectTransform joystick;

    [Space(20)]
    [SerializeField]
    internal string QUIT_KEY = "Quit";

    private TMP_Text messagePanelText;
    private Coroutine timerCoroutine;
    private UIManager uiManager;
    private Vector3 stickStartPos;
    internal event Action<string> OnQuitButtonClick;

    private void Start()
    {
        stickStartPos = joystick.anchoredPosition;
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

    public void ResetStick()
    {
        joystick.anchoredPosition = stickStartPos;
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
