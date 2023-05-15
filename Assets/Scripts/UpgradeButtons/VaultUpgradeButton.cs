using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class VaultUpgradeButton : MonoBehaviour
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
        EventManager.VaultMaxUpgradeReached += OnVaultMaxUpgradeReached;
    }
    void InitState()
    {
        priceText.text = "$" + levelData.vaultUpgradePrice[PlayerPrefs.GetInt("vaultUpgradeIndex")].ToString();
    }

    void Update()
    {
        
    }

    public void OnVaultSizeUpgradePressed()
    {
        EventManager.VaultSizeUpgradePressedEvent();
        priceText.text = "$" + levelData.vaultUpgradePrice[PlayerPrefs.GetInt("vaultUpgradeIndex")].ToString();
    }

    void OnVaultMaxUpgradeReached()
    {
        maxCapacityInterface.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.VaultMaxUpgradeReached -= OnVaultMaxUpgradeReached;
    }
}

