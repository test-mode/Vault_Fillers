using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class WorkerUpgradeButton : MonoBehaviour
{
    // Settings

    // Connections
    public GameObject maxCapacityInterface;
    public LevelData levelData;
    public TextMeshProUGUI priceText;
    
    // State Variables
    

    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {
        EventManager.WorkerMaxUpgradeReached += OnWorkerMaxUpgradeReached;
    }
    void InitState()
    {
        priceText.text = "$" + levelData.workerUpgradePrice[PlayerPrefs.GetInt("workerUpgradeIndex")].ToString();
    }

    void Update()
    {
        
    }

    public void OnWorkerUpgradePressed()
    {
        EventManager.WorkerUpgradePressedEvent();
        priceText.text = "$" + levelData.workerUpgradePrice[PlayerPrefs.GetInt("workerUpgradeIndex")].ToString();
    }

    void OnWorkerMaxUpgradeReached()
    {
        maxCapacityInterface.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.WorkerMaxUpgradeReached -= OnWorkerMaxUpgradeReached;
    }
}

