using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MergeWorkerUpgradeButton : MonoBehaviour
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
        EventManager.MergeWorkerMaxUpgradeReached += OnMergeWorkerMaxUpgradeReached;
    }
    void InitState()
    {
        priceText.text = "$" + levelData.mergeworkerUpgradePrice[PlayerPrefs.GetInt("mergeWorkerUpgradeIndex")].ToString();
    }

    void Update()
    {
        
    }

    public void OnMergeWorkerUpgradePressed()
    {
        EventManager.MergeWorkerUpgradePressedEvent();
        priceText.text = "$" + levelData.mergeworkerUpgradePrice[PlayerPrefs.GetInt("mergeWorkerUpgradeIndex")].ToString();
    }

    void OnMergeWorkerMaxUpgradeReached()
    {
        maxCapacityInterface.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.MergeWorkerMaxUpgradeReached -= OnMergeWorkerMaxUpgradeReached;
    }
}

