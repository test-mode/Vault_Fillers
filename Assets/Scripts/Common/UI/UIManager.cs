using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    const float DEFAULT_START_DELAY = 0.2f;

    public Action OnLevelStart, 
                  OnNextLevel, 
                  OnLevelRestart, 
                  OnGamePaused, 
                  OnGameResumed, 
                  OnInGameRestart;

    [Header("Settings")]
    public bool defaultPauseOperations = true;

    [Header("Screens")]
    public GameObject startCanvas;
    public GameObject ingameCanvas;
    public GameObject finishCanvas;
    public GameObject failCanvas;
    public GameObject pauseMenu;
    [Header("In Game")]
    public LevelBarDisplay levelBarDisplay;
    public TextMeshProUGUI inGameScoreText;
    [Header("Finish Screen")]
    public ScoreTextManager scoreText;
    [Header("Upgrades")]
    public List<GameObject> upgradeButtons;

    // State variables
    float timeScale;

    void Start()
    {
        InitState();
        InitConnections();
    }
    
    void InitState()
    {
        ingameCanvas.SetActive(false);
        timeScale = Time.timeScale;
        inGameScoreText.text = "$" + PlayerPrefs.GetInt("TotalMoney").ToString();
    }
    void InitConnections()
    {
        EventManager.MoneyAmountUpdated += OnMoneyAmountUpdated;
    }

    #region Handler Functions

    public void StartLevelButton()
    {
        OnLevelStart?.Invoke();
        
    }

    public void NextLevelButton()
    {
        PlayerPrefs.SetInt("displayStart", 0);
        OnNextLevel?.Invoke();

    }

    public void RestartLevelButton()
    {
        PlayerPrefs.SetInt("displayStart", 0);
        OnLevelRestart?.Invoke();
    }

    public void OnPauseButtonPressed()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].SetActive(false);
        }
        pauseMenu.SetActive(true); 
        if (defaultPauseOperations)
        {
            timeScale = Time.timeScale; // Restore the current time scale to use in Resume button
            Time.timeScale = 0;
        }
        OnGamePaused?.Invoke();
    }

    public void OnResumeButtonPressed()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].SetActive(true);
        }
        pauseMenu.SetActive(false);
        if (defaultPauseOperations)
        {
            Time.timeScale = timeScale;
        }
        OnGameResumed?.Invoke();
    }

    public void OnInGameRestartPressed()
    {
        if (defaultPauseOperations)
        {
            Time.timeScale = timeScale;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        OnInGameRestart?.Invoke();
    }

    #endregion

    public void StartLevel()
    {
        startCanvas.SetActive(false);
        ingameCanvas.SetActive(true);
    }

    public void SetInGameScore(int score)
    {
        inGameScoreText.text = "$" + score;
    }

    public void SetInGameScoreAsText(string scoreText)
    {
        inGameScoreText.text = scoreText;
    }


    public void DisplayScore(int score, int oldScore=0)
    {
        scoreText.DisplayScore(score, oldScore);
    }

    public void SetLevel(int level)
    {
        levelBarDisplay.SetLevel(level);
    }

    public void UpdateProgess(float progress)
    {
        levelBarDisplay.DisplayProgress(progress);
    }

    public void FinishLevel()
    {
        ingameCanvas.SetActive(false);
        finishCanvas.SetActive(true);
    }

    public void FailLevel()
    {
        ingameCanvas.SetActive(false);
        failCanvas.SetActive(true);
    }
    void InitStates()
    {
        ingameCanvas.SetActive(false);
        finishCanvas.SetActive(false);
        failCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }

  
    public void OnRestartButtonPressed()
    {

    }

    public void OnScreenClicked()
    {

    }

    void Update()
    {
        
    }

    void OnMoneyAmountUpdated()
    {
        SetInGameScore(PlayerPrefs.GetInt("TotalMoney"));
    }

    private void OnDestroy()
    {
        EventManager.MoneyAmountUpdated -= OnMoneyAmountUpdated;
    }
}
