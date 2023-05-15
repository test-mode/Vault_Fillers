using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StackManager : MonoBehaviour
{
    // Settings

    // Connections
    [HideInInspector] public List<GameObject> moneyList = new List<GameObject>();
    [HideInInspector] public GameObject stickman;
    [HideInInspector] public GameObject counter;

    StickmanManager stickmanManager;

    // State Variables
    public int stackCount;

    [HideInInspector] public bool spawnFinished;
    [HideInInspector] public bool stickmanReachedStack;
    [HideInInspector] public bool spawning;

    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {
        
    }
    void InitState()
    {
        spawning = false;
    }

    void Update()
    {

    }

    public void ClearAllMoney()
    {
        for (int i = moneyList.Count - 1; i >= 0; i--)
        {
            Destroy(moneyList[i]);
            moneyList.RemoveAt(i);
        }
    }

    public IEnumerator CollectMoney()
    {
        stickmanManager = stickman.GetComponent<StickmanManager>();

        int stickmanCollectLimit = stickmanManager.stickmanCollectLimit;
        int moneyListCount = moneyList.Count;

        Debug.Log("money list: " + moneyListCount);

        for (int i = moneyListCount - 1; i > moneyListCount - stickmanCollectLimit - 1; i--)
        {
            moneyList[i].GetComponent<MoneyPrefabManager>().stickman = stickman;
            moneyList[i].GetComponent<MoneyPrefabManager>().GetCollected();

            moneyList.RemoveAt(i);

            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(1f);

        stackCount--;
        StickmanEvents.OnMoneyCollected?.Invoke(gameObject);
    }

    public void TransferMoneyObject(GameObject moneyObj)
    {
        moneyList.Add(moneyObj);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == stickman)
        {
            Debug.Log("trigger");
            StickmanEvents.OnReachedCounter?.Invoke(stickman);
        }
    }
}

