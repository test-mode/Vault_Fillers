using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class MoneyPrefabManager : MonoBehaviour
{
    // Settings

    // Connections
    [HideInInspector] public GameObject stickman;

    // State Variables

    [HideInInspector] public float getCollectedDuration;
    [HideInInspector] public Vector3 stickmanHandsPos;
    

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

    }

    void Update()
    {

    }

    public void GetCollected()
    {
        if (stickman != null)
        {
            stickmanHandsPos = stickman.GetComponentInChildren<SkinnedMeshRenderer>().bounds.center;
            transform.DOMove(stickmanHandsPos, getCollectedDuration);

            transform.DOScale(.5f, getCollectedDuration).OnComplete(() =>
            {
                stickman.GetComponent<StickmanManager>().StackCollectedMoney(gameObject);
            });
        }
    }
}

