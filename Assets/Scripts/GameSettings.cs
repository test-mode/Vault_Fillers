using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ambrosia.StateMachine;
using System.Collections;

public class GameSettings : MonoBehaviour
{
    // Settings
    [SerializeField] private bool saveGame;

    // Connections
    [Header("Connections")]
    public GameObject vault;
    public QueueManager queueManager;
    public GameObject moneyStacksParent;
    public GameObject moneyCountersParent;
    [Header("Prefabs")]
    public List<GameObject> counterUpgrades = new List<GameObject>();
    public LevelData levelData;
    public GameObject stickmanPrefab;
    public GameObject counterPrefab;

    // State Variables
    [HideInInspector] public List<GameObject> stickmans = new List<GameObject>();
    List<GameObject> moneyStacks = new List<GameObject>();
    List<GameObject> moneyCounters = new List<GameObject>();
    List<GameObject> counterPositions = new List<GameObject>();
    int countersCount;

    int totalMoney;

    int vaultMaxUpgradeCount;
    int workerMaxUpgradeCount;
    int counterMaxUpgradeCount;
    int mergeWorkerMaxUpgradeCount;
    int mergeCounterMaxUpgradeCount;

    int vaultUpgradeIndex;
    int workerUpgradeIndex;
    int counterUpgradeIndex;
    int mergeWorkerUpgradeIndex;
    int mergeCounterUpgradeIndex;

    bool workerUpgradeMaxReached;
    bool counterUpgradeMaxReached;
    bool vaultUpgradeMaxReached;
    bool mergeWorkerUpgradeMaxReached;
    bool mergeCounterUpgradeMaxReached;

    [Header("Variables")]
    public float vaultSizeUpgradeMagnitude;

    private void Awake()
    {
        CountersInitialSetup();
        StickmansInitialSetup();
    }

    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {
        EventSubscribe();

        vaultMaxUpgradeCount = levelData.vaultUpgradePrice.Count;
        workerMaxUpgradeCount = levelData.workerUpgradePrice.Count;
        counterMaxUpgradeCount = levelData.counterUpgradePrice.Count;
        mergeWorkerMaxUpgradeCount = levelData.mergeworkerUpgradePrice.Count;
        mergeCounterMaxUpgradeCount = levelData.mergeCounterUpgradePrice.Count;
    }
    void InitState()
    {
        countersCount = 0;
        GetSavedValues();
        UpdateMoneyAmount();
    }

    private void GetSavedValues()
    {
        totalMoney = PlayerPrefs.GetInt("TotalMoney", 10);

        vaultUpgradeIndex = PlayerPrefs.GetInt("vaultUpgradeIndex", 0);
        workerUpgradeIndex = PlayerPrefs.GetInt("workerUpgradeIndex", 0);
        counterUpgradeIndex = PlayerPrefs.GetInt("counterUpgradeIndex", 0);
        mergeWorkerUpgradeIndex = PlayerPrefs.GetInt("mergeWorkerUpgradeIndex", 0);
        mergeCounterUpgradeIndex = PlayerPrefs.GetInt("mergeCounterUpgradeIndex", 0);
    }

    void StickmansInitialSetup()
    {
        for (int i = 0; i < workerUpgradeIndex; i++)
        {
            UpgradeWorker();
        }
    }
    void CountersInitialSetup()
    {
        moneyCounters.Clear();

        //Transfer children in MoneyCounters object in scene to moneyCounters list
        for (int i = 0; i < moneyCountersParent.transform.childCount; i++)
        {
            moneyCounters.Add(null);
            moneyStacks.Add(moneyStacksParent.transform.GetChild(i).gameObject);
            moneyCountersParent.transform.GetChild(i).gameObject.SetActive(false);
            counterPositions.Add(moneyCountersParent.transform.GetChild(i).gameObject);
        }

        //Activate counters at start
        for (int i = 0; i < counterUpgradeIndex; i++)
        {
            UpgradeCounter();
        }
    }

    void Update()
    {
        #region Cheats
        if (Input.GetKeyDown(KeyCode.M))
        {
            UpdateMoneyAmount(1234);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerPrefs.DeleteAll();
        }
        #endregion

    }

    private void OnDestroy()
    {
        EventUnsubscribe();

        if (!saveGame)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void UpdateMoneyAmount(int amount = 0)
    {
        totalMoney += amount;
        PlayerPrefs.SetInt("TotalMoney", totalMoney);
        EventManager.MoneyAmountUpdatedEvent();
    }

    void UpdateUpgradeIndex(string parameter, ref int index)
    {
        index++;
        PlayerPrefs.SetInt(parameter, index);
    }

    GameObject SpawnStickman()
    {
        Vector3 position = vault.transform.position + new Vector3(-10, 0, 0);
        var stickman = Instantiate(stickmanPrefab, position, Quaternion.identity);
        stickmans.Add(stickman);

        stickman.GetComponent<StickmanManager>().vault = vault;
        stickman.GetComponent<StickmanManager>().queueManager = queueManager;

        return stickman;
    } 

    GameObject SpawnCounter(GameObject prefab)
    {
        for (int i = 0; i < counterPositions.Count; i++)
        {
            if (moneyCounters[i] == null)
            {
                var counter = Instantiate(prefab, counterPositions[i].transform.position, Quaternion.identity);
                counter.transform.Rotate(0, 180, 0);

                counter.GetComponent<MoneySpawner>().personalStack = moneyStacks[i];
                moneyStacks[i].GetComponent<StackManager>().counter = counter;
                moneyCounters[i] = counter;
                moneyCounters.Add(counter);

                for (int y = 0; y < stickmans.Count; y++)
                {
                    if (stickmans[y].GetComponent<StickmanManager>().moneyStack == null)
                    {
                        stickmans[y].GetComponent<StickmanManager>().moneyStack = counter.GetComponent<MoneySpawner>().personalStack;
                    }
                }

                return counter;
            }
        }

        return null;
    }

    //Upgrades
    void UpgradeWorker()
    {
        var stickman = SpawnStickman();

        for (int i = 0; i < moneyStacks.Count; i++)
        {
            if (moneyStacks[i].GetComponent<StackManager>().stickman == null)
            {
                stickman.GetComponent<StickmanManager>().moneyStack = moneyStacks[i];
                moneyStacks[i].GetComponent<StackManager>().stickman = stickman;
                break;
            }
        }
    }
    void UpgradeCounter()
    {
        SpawnCounter(counterPrefab);
        countersCount++;
    }
    void UpgradeVault()
    {
        vault.transform.DOScale(vault.transform.localScale * vaultSizeUpgradeMagnitude, 1);
    }
    void MergeWorker()
    {
        
    }
    void MergeCounter()
    {
        var movePosition = moneyCounters[mergeCounterUpgradeIndex].transform.position;
        for (int i = mergeCounterUpgradeIndex; i < mergeCounterUpgradeIndex + 3; i++)
        {
            var counter = moneyCounters[i];
            counter.transform.DOMove(movePosition, 1f).OnComplete(() =>
            {
                Destroy(counter);
            });

            countersCount -= 2;
        }

        for (int i = moneyCounters.Count - 1; i > 0; i--)
        {
            if (moneyCounters[i] == null)
            {
                moneyCounters.RemoveAt(i);
            }
        }

        StartCoroutine(SpawnCounterWithDelay(3f));
    }

    IEnumerator SpawnCounterWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        SpawnCounter(counterUpgrades[1]);
    }


    #region Events

    void EventSubscribe()
    {
        EventManager.VaultSizeUpgradePressed += OnVaultSizeUpgradePressed;
        EventManager.WorkerUpgradePressed += OnWorkerUpgradePressed;
        EventManager.MergeWorkerUpgradePressed += OnMergeWorkerUpgradePressed;
        EventManager.MoneyCounterUpgradePressed += OnMoneyCounterUpgradePressed;
        EventManager.MergeCounterUpgradePressed += OnMergeCounterUpgradePressed;

        EventManager.MoneyDeposited += OnMoneyDeposited;
    }
    void EventUnsubscribe()
    {
        EventManager.VaultSizeUpgradePressed -= OnVaultSizeUpgradePressed;
        EventManager.WorkerUpgradePressed -= OnWorkerUpgradePressed;
        EventManager.MergeWorkerUpgradePressed -= OnMergeWorkerUpgradePressed;
        EventManager.MoneyCounterUpgradePressed -= OnMoneyCounterUpgradePressed;
        EventManager.MergeCounterUpgradePressed -= OnMergeCounterUpgradePressed;

        EventManager.MoneyDeposited -= OnMoneyDeposited;
    }

    void OnMoneyDeposited()
    {
        UpdateMoneyAmount(levelData.moneyValue);
    }

    void OnWorkerUpgradePressed()
    {
        if (workerUpgradeIndex < workerMaxUpgradeCount && !workerUpgradeMaxReached)
        {
            if (totalMoney >= levelData.workerUpgradePrice[workerUpgradeIndex])
            {
                UpgradeWorker();
                UpdateMoneyAmount(-levelData.workerUpgradePrice[workerUpgradeIndex]);
                if (workerUpgradeIndex + 1 < workerMaxUpgradeCount)
                {
                    UpdateUpgradeIndex(nameof(workerUpgradeIndex), ref workerUpgradeIndex);
                }
                else
                {
                    workerUpgradeMaxReached = true;
                    EventManager.WorkerMaxUpgradeReachedEvent();
                }
            }
        }
    }
    void OnMoneyCounterUpgradePressed()
    {
        if (counterUpgradeIndex < counterMaxUpgradeCount && !counterUpgradeMaxReached)
        {
            if (totalMoney >= levelData.counterUpgradePrice[counterUpgradeIndex])
            {
                UpgradeCounter();
                UpdateMoneyAmount(-levelData.counterUpgradePrice[counterUpgradeIndex]);

                if (counterUpgradeIndex + 1 < counterMaxUpgradeCount)
                {
                    UpdateUpgradeIndex(nameof(counterUpgradeIndex), ref counterUpgradeIndex);
                }
                else
                {
                    counterUpgradeMaxReached = true;
                    EventManager.MoneyCounterMaxUpgradeReachedEvent();
                }
            }
        }
    }
    void OnVaultSizeUpgradePressed()
    {
        if (vaultUpgradeIndex < vaultMaxUpgradeCount && !vaultUpgradeMaxReached)
        {
            if (totalMoney >= levelData.vaultUpgradePrice[vaultUpgradeIndex])
            {
                UpgradeVault();
                UpdateMoneyAmount(-levelData.vaultUpgradePrice[vaultUpgradeIndex]);
                if (vaultUpgradeIndex + 1 < vaultMaxUpgradeCount)
                {
                    UpdateUpgradeIndex(nameof(vaultUpgradeIndex), ref vaultUpgradeIndex);
                }
                else
                {
                    vaultUpgradeMaxReached = true;
                    EventManager.VaultMaxUpgradeReachedEvent();
                }
            }
        }
    }
    void OnMergeWorkerUpgradePressed()
    {
        if (mergeWorkerUpgradeIndex < mergeWorkerMaxUpgradeCount && !mergeWorkerUpgradeMaxReached)
        {
            if (totalMoney >= levelData.mergeworkerUpgradePrice[mergeWorkerUpgradeIndex])
            {
                MergeWorker();
                UpdateMoneyAmount(-levelData.mergeworkerUpgradePrice[mergeWorkerUpgradeIndex]);
                if (mergeWorkerUpgradeIndex + 1 < mergeWorkerMaxUpgradeCount)
                {
                    UpdateUpgradeIndex(nameof(mergeWorkerUpgradeIndex), ref mergeWorkerUpgradeIndex);
                }
                else
                {
                    mergeWorkerUpgradeMaxReached = true;
                    EventManager.MergeWorkerMaxUpgradeReachedEvent();
                }
            }
        }
    }
    void OnMergeCounterUpgradePressed()
    {
        if (mergeCounterUpgradeIndex < mergeCounterMaxUpgradeCount && !mergeCounterUpgradeMaxReached)
        {
            if (totalMoney >= levelData.mergeCounterUpgradePrice[mergeCounterUpgradeIndex])
            {
                MergeCounter();
                UpdateMoneyAmount(-levelData.mergeCounterUpgradePrice[mergeCounterUpgradeIndex]);
                if (mergeCounterUpgradeIndex + 1 < mergeCounterMaxUpgradeCount)
                {
                    UpdateUpgradeIndex(nameof(mergeCounterUpgradeIndex), ref mergeCounterUpgradeIndex);
                }
                else
                {
                    mergeCounterUpgradeMaxReached = true;
                    EventManager.MergeCounterMaxUpgradeReachedEvent();
                }
            }
        }
    }

    #endregion
}