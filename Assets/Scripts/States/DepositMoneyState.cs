using UnityEngine;
using Ambrosia.StateMachine;

public class DepositMoneyState : State
{
    public GameObject stickman;
    public StickmanManager stickmanManager;
    public State goToCounterState;

    protected override void OnEnter()
    {
        StickmanEvents.OnMoneyDeposited += OnMoneyDeposited;
        StartCoroutine(stickmanManager.DepositMoneyToVault());
    }

    protected override void OnExit()
    {
        StickmanEvents.OnMoneyDeposited -= OnMoneyDeposited;
        stickmanManager.queueManager.RemoveFromQueue(stickmanManager.gameObject);
    }

    private void Update()
    {

    }

    void OnMoneyDeposited(GameObject stickman)
    {
        if (stickmanManager.gameObject == stickman)
        {
            StateMachine.TransitionTo(goToCounterState);
        }
        
    }
}