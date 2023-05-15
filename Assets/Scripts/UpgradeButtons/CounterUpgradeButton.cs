using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CounterUpgradeButton : MonoBehaviour
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
        EventManager.MoneyCounterMaxUpgradeReached += OnCounterMaxUpgradeReached;
    }
    void InitState()
    {
        priceText.text = "$" + levelData.counterUpgradePrice[PlayerPrefs.GetInt("counterUpgradeIndex")].ToString();
    }

    void Update()
    {
        
    }

    public void OnMoneyCounterUpgradePressed()
    {
        EventManager.MoneyCounterUpgradePressedEvent();
        priceText.text = "$" + levelData.counterUpgradePrice[PlayerPrefs.GetInt("counterUpgradeIndex")].ToString();
    }

    void OnCounterMaxUpgradeReached()
    {
        maxCapacityInterface.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.MoneyCounterUpgradePressed -= OnCounterMaxUpgradeReached;
    }
}

