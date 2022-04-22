using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField]
    internal GameObject startButtonObj;
    [SerializeField]
    internal TextMeshProUGUI clearProgressButton;
    [SerializeField]
    internal TextMeshProUGUI backButton;
    [SerializeField]
    internal TextMeshProUGUI difficulty;
    [SerializeField]
    internal TextMeshProUGUI levelDifficulty;
    [SerializeField]
    private TextMeshProUGUI startText;
    [SerializeField]
    private GameObject levelButtonPrefab;
    [SerializeField]
    private GameObject levelList;
    [SerializeField]
    private GameObject levelDetails;
    [SerializeField]
    private LevelInfoHandler levelInfoHandler;

    [Space(20)]
    [SerializeField]
    internal string START_KEY = "Start";
    [SerializeField]
    internal string RESET_KEY = "ResetProgress";
    [SerializeField]
    internal string BACK_KEY = "Back";
    [SerializeField]
    internal string LEVEL_NAME_KEY = "Level";
    [SerializeField]
    internal string LEVEL_DIFFICULTY_KEY = "LevelDifficulty";
    [SerializeField]
    internal string LEVEL_EASY_KEY = "Easy";
    [SerializeField]
    internal string LEVEL_MEDIUM_KEY = "Medium";
    [SerializeField]
    internal string LEVEL_HARD_KEY = "Hard";

    private Button startButton;
    internal string levelName;
    private int? levelId = null;
    private List<GameObject> buttons = new List<GameObject>();

    internal Action<int?> OnStartButtonClick;
    internal Action<string> OnClearButtonClick;
    internal Action<GameState> OnBackButtonClick;

    private void Awake()
    {
        startButton = startButtonObj.GetComponent<Button>();
        startButton.interactable = false;
        levelDetails.SetActive(false);
    }

    private void OnLevelClick(string stringId)
    {
        levelId = int.Parse(stringId) - 1;
        DisplayLevelInfo(levelId);
        startButton.interactable = true;
    }

    public void OnStartClick()
    {
        OnStartButtonClick?.Invoke(levelId);
    }

    public void OnClearClick()
    {
        OnClearButtonClick?.Invoke(tag);
    }

    public void OnBackClick()
    {
        var state = GameState.MainMenu;
        OnBackButtonClick?.Invoke(state);
        DestroyLevelList();
        levelDetails.SetActive(false);
        startButton.interactable = false;
        levelId = null;
    }

    public void UpdateMenu(string start, string clear, string back, string diff)
    {
        startText.text = start;
        clearProgressButton.text = clear;
        backButton.text = back;
        difficulty.text = diff;
    }

    internal void FillLevelList(List<Level> levels)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject button = Instantiate(levelButtonPrefab, levelList.transform, false);
            TMP_Text buttonName = button.GetComponentInChildren<TMP_Text>();
            var buttonComponent = button.GetComponentInChildren<Button>();
            buttonName.text = $"{levelName} {i + 1}";
            buttonComponent.interactable = levels[i].IsOpen;
            buttonComponent.name = (i + 1).ToString();
            buttonComponent.onClick.AddListener(() => OnLevelClick(buttonComponent.name));
            buttons.Add(button);
        }
    }

    internal void DestroyLevelList()
    {
        foreach (var item in buttons)
        {
            DestroyImmediate(item);
        }
        buttons.Clear();
    }

    internal void UpdateLevelList()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var buttonComponent = buttons[i].GetComponent<Button>();
            if (levelInfoHandler.levels[i].IsOpen)
            {
                buttonComponent.interactable = true;
            }
            else
            {
                buttonComponent.interactable = false;
            }
        }
    }

    private void DisplayLevelInfo(int? id)
    {
        if (!levelDetails.activeSelf)
        {
            levelDetails.SetActive(true);
        }
        levelDifficulty.text = levelInfoHandler.levels[(int)id].DifficultyString;
    }
}
