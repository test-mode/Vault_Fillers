using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoneySpawner : MonoBehaviour
{
    // Settings
    //[HideInInspector] public List<GameObject> moneyList = new List<GameObject>();
    public static Action<GameObject> OnCounterDestroyed;

    // Connections
    [HideInInspector] public GameObject personalStack;
    StackManager stackManager;
    
    [Header("Connections")]
    public GameObject moneyPrefab;
    public GameObject gridCenter;

    // State Variables
    [Header("Grid Settings")]
    [Range(1, 20)]
    public int gridLength;
    [Range(1, 20)]
    public int gridWidth;
    [Range(1, 20)]
    public int gridHeight;

    [Header("Variables")]
    public int stackLimit;

    float spawnTime;
    float spawnTimer;

    [Header("Timings")]
    public float waitBetweenSpawn;
    public float moneyMoveTime;

    [Header("Money Prefab Settings")]
    public float moneyGetCollectedDuration;

    public float period;
    [HideInInspector] public bool spawnFinished;
    [HideInInspector] public bool collecting;

    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {
        stackManager = personalStack.GetComponent<StackManager>();
        StickmanEvents.OnCollecting += OnCollecting;
        StickmanEvents.OnMoneyCollected += OnMoneyCollected;
    }
    void InitState()
    {
        //collecting = false;
        period = 10f;

        spawnTimer = 0f;
        spawnTime = 0f;


        Debug.Log(stackManager.stackCount);
        Debug.Log(collecting);

        InvokeRepeating(nameof(Spawner), 0f, 10f);
    }

    void Update()
    {
        //SpawnRepeating();
    }

    void SpawnRepeating()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTime && !collecting)
        {
            if (stackManager.stackCount < stackLimit)
            {
                personalStack.GetComponent<StackManager>().spawning = true;
                StartCoroutine(SpawnInGrid());
                spawnTimer = 0f;
                spawnTime = 10f;
            }
        }
    }

    void Spawner()
    {
        if (stackManager.stackCount < stackLimit && !collecting)
        {
            personalStack.GetComponent<StackManager>().spawning = true;
            StartCoroutine(SpawnInGrid());
        }
    }

    public IEnumerator SpawnInGrid()
    {
        stackManager = personalStack.GetComponent<StackManager>();

        for (int y = 0; y < gridHeight; y++)
        {
            for (int z = 0; z < gridLength; z++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    GameObject moneyObj = InstantiateMoney();
                    moneyObj.GetComponent<Rigidbody>().isKinematic = true;

                    float prefabWidth = moneyObj.GetComponent<Collider>().bounds.size.x;
                    float prefabHeight = moneyObj.GetComponent<Collider>().bounds.size.y;
                    float prefabLength = moneyObj.GetComponent<Collider>().bounds.size.z;

                    float width = gridCenter.transform.position.x + x * prefabWidth;
                    float heigth = gridCenter.transform.position.y + (y + stackManager.stackCount) * prefabHeight;
                    float length = gridCenter.transform.position.z + z * prefabLength;

                    width -= gridWidth / 2f + prefabWidth / 4f;
                    length -= gridLength / 2f + prefabLength / 4f;

                    Vector3 posInGrid = new Vector3(width, heigth, length);
                    moneyObj.transform.DOMove(posInGrid, moneyMoveTime).OnComplete(() =>
                    {
                        MoneyPrefabConnections(moneyObj);
                    });

                    yield return new WaitForSeconds(waitBetweenSpawn);
                }
            }
        }
        yield return new WaitForSeconds(1f);

        CounterEvents.OnSpawnFinished?.Invoke(personalStack);
        stackManager.stackCount++;
        personalStack.GetComponent<StackManager>().spawning = false;
    }

    void OnCollecting(GameObject stack)
    {
        if (stack == personalStack)
        {
            collecting = true;
        }
    }

    void OnMoneyCollected(GameObject stack)
    {
        if (stack == personalStack)
        {
            collecting = false;
        }
    }

    private void OnDestroy()
    {
        CounterEvents.OnCounterDestroyed?.Invoke(personalStack);
        StickmanEvents.OnCollecting -= OnCollecting;
        StickmanEvents.OnMoneyCollected -= OnMoneyCollected;
    }

    #region Prefab settings

    GameObject InstantiateMoney()
    {
        GameObject moneyObj = Instantiate(moneyPrefab, transform.position, Quaternion.identity);

        return moneyObj;
    }

    void MoneyPrefabConnections(GameObject moneyObj)
    {
        moneyObj.GetComponent<MoneyPrefabManager>().getCollectedDuration = moneyGetCollectedDuration;
        personalStack.GetComponent<StackManager>().TransferMoneyObject(moneyObj);
    }

    #endregion
}