using UnityEngine;
using Ambrosia.StateMachine;
using UnityEngine.AI;

public class JoinQueueState : State
{
    [HideInInspector] public Vector3 positionInQueue;
    public StickmanManager stickmanManager;
    public NavMeshAgent navMeshAgent;

    protected override void OnEnter()
    {
        StickmanEvents.OnReachedDepositPosition += OnReachedDepositPosition;
        stickmanManager.queueManager.AddToQueue(stickmanManager.gameObject);
    }

    protected override void OnExit()
    {
        StickmanEvents.OnReachedDepositPosition -= OnReachedDepositPosition;
    }

    private void Update()
    {
        navMeshAgent.SetDestination(positionInQueue);
    }

    void OnReachedDepositPosition (GameObject stickman)
    {
        if (stickmanManager.gameObject == stickman)
        {
            StateMachine.TransitionToNextState();
        }
    }
}