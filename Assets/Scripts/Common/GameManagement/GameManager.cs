using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Settings
    public bool spawnLevel = true;
    public int tutorialLevels;

    // Connections
    public GameObject[] levels;
    public UIManager ui;

    // State variables
    int currentLevel;
    int score;
    int totalDepositedMoney;

    void Awake()
    {
        InitStates();
        InitConnections();
    }

    void InitStates()
    {
        currentLevel = PlayerPrefs.GetInt("Level", 0);
        LoadLevel();
    }

    void LoadLevel()
    {
        if (spawnLevel)
        {
            int prefabIndex = GetPrefabIndex(currentLevel, tutorialLevels, levels.Length);
            GameObject levelGO = Instantiate(levels[prefabIndex], Vector3.zero, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < 50; i++)
            {
                Debug.Log("Prefab index for level " + i + ":" + GetPrefabIndex(i, tutorialLevels, levels.Length));
            }
        }
    }

    int GetPrefabIndex(int levelIndex, int nInitialLevels, int nLevels)
    {

        int nRepeatingLevels = nLevels - nInitialLevels;
        int prefabIndex = levelIndex;
        if (levelIndex >= nInitialLevels)
        {
            //prefabIndex = ((levelIndex - nInitialLevels) % nRepeatingLevels) + nInitialLevels; TODO Attempted to divide by zero error.
        }
        return prefabIndex;

    }

    void InitConnections()
    {
        ui.OnLevelStart += OnLevelStart;
        ui.OnNextLevel += OnNextLevel;
        ui.OnLevelRestart += OnLevelRestart;

        EventManager.ScreenClicked += ScreenClicked;
        EventManager.MoneyDeposited += MoneyDeposited;
    }

    void OnLevelFailed()
    {
        ui.FailLevel();
        Debug.Log("LEVEL FAILED");
        
    }

    void OnFinishLevel()
    {
        ui.FinishLevel();
        PlayerPrefs.SetInt("Level", currentLevel + 1);
    }

    void OnLevelStart()
    {
        //Debug.Log("LEVEL STARTED");
    }

    void OnLevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("showStart", 0);
    }

    void ScreenClicked()
    {

    }
    void MoneyDeposited()
    {
        totalDepositedMoney += 1;
        ui.inGameScoreText.text = "$" + totalDepositedMoney.ToString();
    }

    void OnDestroy()
    {
        EventManager.ScreenClicked -= ScreenClicked;
        EventManager.MoneyDeposited -= MoneyDeposited;
    }
}