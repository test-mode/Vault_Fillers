using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "Level Data", order = 51)]

public class LevelData : ScriptableObject
{
    [Header("Prices")]
    public List<int> workerUpgradePrice = new List<int>();
    public List<int> counterUpgradePrice = new List<int>();
    public List<int> vaultUpgradePrice = new List<int>();
    public List<int> mergeworkerUpgradePrice = new List<int>();
    public List<int> mergeCounterUpgradePrice = new List<int>();


    [Header("Money")]
    public int moneyValue;
}

