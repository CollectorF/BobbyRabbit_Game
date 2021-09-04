using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField]
    internal TMP_Text startButton;
    [SerializeField]
    internal TMP_Text clearProgressButton;
    [SerializeField]
    internal TMP_Text backButton;
    [SerializeField]
    internal TMP_Text difficulty;
    [SerializeField]
    internal TMP_Text levelDifficulty;
    [SerializeField]
    private GameObject levelButtonPrefab;
    [SerializeField]
    private GameObject levelList;
    [SerializeField]
    private GameObject levelDetails;
    [SerializeField]
    private LevelLoader levelLoader;

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

    internal string levelName;

    private List<GameObject> buttons = new List<GameObject>();
    internal Action OnBackButtonClick;

    private void Start()
    {
        levelDetails.SetActive(false);
    }

    public void OnBackClick()
    {
        OnBackButtonClick?.Invoke();
        foreach (var item in buttons)
        {
            DestroyImmediate(item);
        }
        levelDetails.SetActive(false);
    }

    public void UpdateMenu(string start, string clear, string back, string diff)
    {
        startButton.text = start;
        clearProgressButton.text = clear;
        backButton.text = back;
        difficulty.text = diff;
    }

    internal void FillLevelList(List<Level> levels)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject button = Instantiate(levelButtonPrefab);
            button.transform.SetParent(levelList.transform);
            TMP_Text buttonName = button.GetComponentInChildren<TMP_Text>();
            var buttonComponent = button.GetComponentInChildren<Button>();
            buttonName.text = $"{levelName} {i + 1}";
            //buttonComponent.interactable = levels[i].IsOpen;
            buttonComponent.name = (i + 1).ToString();
            buttonComponent.onClick.AddListener(() => LevelButtonClicked(buttonComponent.name));
            buttons.Add(button);
        }
    }

    private void LevelButtonClicked(string stringId)
    {
        int levelId = int.Parse(stringId) - 1;
        DisplayLevelInfo(levelId);
    }

    private void DisplayLevelInfo(int id)
    {
        if (!levelDetails.activeSelf)
        {
            levelDetails.SetActive(true);
        }
        levelDifficulty.text = levelLoader.levels[id].DifficultyString;
    }
}
