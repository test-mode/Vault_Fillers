using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MergeCounterUpgradeButton : MonoBehaviour
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
        EventManager.MergeCounterMaxUpgradeReached += OnMergeCounterMaxUpgradeReached;
    }
    void InitState()
    {
        priceText.text = "$" + levelData.mergeCounterUpgradePrice[PlayerPrefs.GetInt("mergeCounterUpgradeIndex")].ToString();
    }

    void Update()
    {
        
    }

    public void OnMergeCounterUpgradePressed()
    {
        EventManager.MergeCounterUpgradePressedEvent();
        priceText.text = "$" + levelData.mergeCounterUpgradePrice[PlayerPrefs.GetInt("mergeCounterUpgradeIndex")].ToString();
    }

    void OnMergeCounterMaxUpgradeReached()
    {
        maxCapacityInterface.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.MergeCounterMaxUpgradeReached -= OnMergeCounterMaxUpgradeReached;
    }
}

