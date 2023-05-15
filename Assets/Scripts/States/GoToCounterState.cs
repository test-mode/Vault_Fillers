using UnityEngine;
using Ambrosia.StateMachine;
using UnityEngine.AI;

public class GoToCounterState : State
{
    public StickmanManager stickmanManager;
    public NavMeshAgent navMeshAgent;

    protected override void OnEnter()
    {
        StickmanEvents.OnReachedCounter += OnReachedCounter;
        navMeshAgent.SetDestination(stickmanManager.moneyStack.transform.position + new Vector3(2, 0, 0));
    }

    protected override void OnExit()
    {
        StickmanEvents.OnReachedCounter -= OnReachedCounter;
    }

    private void Update()
    {

    }

    void OnReachedCounter(GameObject stickman)
    {
        if (stickmanManager.gameObject == stickman)
        {
            StateMachine.TransitionToNextState();
        }
    }
}
