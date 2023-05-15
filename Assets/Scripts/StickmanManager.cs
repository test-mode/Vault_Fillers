using Ambrosia.StateMachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StickmanManager : MonoBehaviour
{
    // Settings

    // Connections
    //[HideInInspector] public GameObject personalMoneyCounter;
    [HideInInspector] public Vector3 moneyDepositDestination;
    [HideInInspector] public GameObject moneyStack;
    [HideInInspector] public GameObject vault;
    [HideInInspector] public QueueManager queueManager;

    [Header("Connections")]
    public GameObject moneyStackPos;
    [SerializeField] StateMachine stateMachine;

    VaultManager vaultManager;
    NavMeshAgent navMeshAgent;
    Animator stickmanAnimator;

    [HideInInspector] public bool spawnFinished = false;
    bool stackFull = false;
    

    // State Variables
    //list
    [HideInInspector] public List<GameObject> stackedMoney = new List<GameObject>();

    [Header("Variables")]
    //float
    public float movementSpeed;
    public float timeBetweenDeposits;
    public float distanceToStartDeposit;


    //int
    [HideInInspector] public int stickmanCollectLimit;

    //bool
    bool isCarrying;
    bool isRunning;


    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {
        CounterEvents.OnCounterDestroyed += OnCounterDestroyed;
        CounterEvents.OnSpawnFinished += OnSpawnFinished;

        stickmanAnimator = GetComponent<Animator>();
        vaultManager = vault.GetComponent<VaultManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    void InitState()
    {
        SetIdleAnimation();

        isCarrying = false;

        stickmanCollectLimit = 24;
    }

    void Update()
    {
        SetAnimations();
        DetermineStickmanState();

        if (ReachedDestination())
        {
            if (Vector3.Distance(transform.position, moneyStack.transform.position + new Vector3(2, 0, 0)) < 1f && !moneyStack.GetComponent<StackManager>().spawning)
            {
                StickmanEvents.OnCollecting?.Invoke(moneyStack);
                StickmanEvents.OnReachedCounter?.Invoke(gameObject);
            }
        }
    }

    void DetermineStickmanState()
    {
        if (stackedMoney.Count > 0)
        {
            isCarrying = true;
        }
        else
        {
            isCarrying = false;
        }
    }

    public bool ReachedDestination()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public IEnumerator DepositMoneyToVault()
    {
        for (int i = stackedMoney.Count - 1; i >= 0; i = stackedMoney.Count - 1)
        {
            stackedMoney[i].transform.DOMove(vault.transform.position, timeBetweenDeposits).OnComplete(() =>
            {
                Destroy(stackedMoney[i]);
                stackedMoney.Remove(stackedMoney[i]);
                vaultManager.VaultPunchScaleAnimation(timeBetweenDeposits);
                EventManager.MoneyDepositedEvent();
            });

            yield return new WaitForSeconds(timeBetweenDeposits + 0.01f);
        }
        StickmanEvents.OnMoneyDeposited?.Invoke(gameObject);
    }

    public void StackCollectedMoney(GameObject moneyObj)
    {
        moneyObj.transform.rotation = moneyStackPos.transform.rotation;
        moneyObj.GetComponent<Rigidbody>().isKinematic = true;
        moneyObj.transform.localScale = Vector3.one;
        moneyObj.transform.SetParent(moneyStackPos.transform);

        float x = moneyStackPos.transform.localPosition.x;
        float y = moneyObj.GetComponentInChildren<MeshRenderer>().bounds.size.y * stackedMoney.Count;
        float z = moneyStackPos.transform.localPosition.z;

        moneyObj.transform.localPosition = new Vector3(x, y, z);
        stackedMoney.Add(moneyObj);
    }

    void OnCounterDestroyed(GameObject counter)
    {
        
    }

    void OnSpawnFinished(GameObject stack)
    {
        if (stack == moneyStack)
        {
            spawnFinished = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DepositPosition"))
        {
            StickmanEvents.OnReachedDepositPosition?.Invoke(gameObject);
        }
    }

    private void OnDestroy()
    {
        CounterEvents.OnCounterDestroyed -= OnCounterDestroyed;
        CounterEvents.OnSpawnFinished -= OnSpawnFinished; 
    }


    #region Animations

    void SetAnimations()
    {
        bool isIdle = navMeshAgent.velocity.sqrMagnitude == 0;
        if (isIdle)
        {
            if (isCarrying)
            {
                SetCarryIdleAnimation();
            }
            else
            {
                SetIdleAnimation();
            }
        }
        else
        {
            if (isRunning)
            {
                if (isCarrying)
                {
                    SetCarryWalkAnimation();
                }
                else
                {
                    SetRunAnimation();
                }
            }
            else
            {
                if (isCarrying)
                {
                    SetCarryWalkAnimation();
                }
                else
                {
                    SetWalkAnimation();
                }
            }
        }
    }

    void SetIdleAnimation()
    {
        stickmanAnimator.SetBool("Idle", true);

        stickmanAnimator.SetBool("Walk", false);
        stickmanAnimator.SetBool("Run", false);
        stickmanAnimator.SetBool("CarryIdle", false);
        stickmanAnimator.SetBool("CarryWalk", false);
        stickmanAnimator.SetBool("Win", false);
    }
    void SetWalkAnimation()
    {
        stickmanAnimator.SetBool("Walk", true);

        stickmanAnimator.SetBool("Idle", false);
        stickmanAnimator.SetBool("Run", false);
        stickmanAnimator.SetBool("CarryIdle", false);
        stickmanAnimator.SetBool("CarryWalk", false);
        stickmanAnimator.SetBool("Win", false);
    }
    void SetRunAnimation()
    {
        stickmanAnimator.SetBool("Run", true);

        stickmanAnimator.SetBool("Idle", false);
        stickmanAnimator.SetBool("Walk", false);
        stickmanAnimator.SetBool("CarryIdle", false);
        stickmanAnimator.SetBool("CarryWalk", false);
        stickmanAnimator.SetBool("Win", false);
    }
    void SetCarryIdleAnimation()
    {
        stickmanAnimator.SetBool("CarryIdle", true);

        stickmanAnimator.SetBool("Idle", false);
        stickmanAnimator.SetBool("Walk", false);
        stickmanAnimator.SetBool("Run", false);
        stickmanAnimator.SetBool("CarryWalk", false);
        stickmanAnimator.SetBool("Win", false);
    }
    void SetCarryWalkAnimation()
    {
        stickmanAnimator.SetBool("CarryWalk", true);

        stickmanAnimator.SetBool("Idle", false);
        stickmanAnimator.SetBool("Walk", false);
        stickmanAnimator.SetBool("Run", false);
        stickmanAnimator.SetBool("CarryIdle", false);
        stickmanAnimator.SetBool("Win", false);
    }
    void SetWinAnimation()
    {
        stickmanAnimator.SetBool("Win", true);

        stickmanAnimator.SetBool("Idle", false);
        stickmanAnimator.SetBool("Walk", false);
        stickmanAnimator.SetBool("Run", false);
        stickmanAnimator.SetBool("CarryIdle", false);
        stickmanAnimator.SetBool("CarryWalk", false);
    }

    #endregion
}