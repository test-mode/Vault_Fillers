using UnityEngine;
using Ambrosia.StateMachine;

public class WaitState : State
{
    public StickmanManager stickmanManager;
    public State collectMoneyState;

    protected override void OnEnter()
    {
        Debug.Log("on wait");
    }

    protected override void OnExit()
    {

    }

    private void Update()
    {
        if (stickmanManager.moneyStack.GetComponent<StackManager>().moneyList.Count > 0)
        {
            StateMachine.TransitionTo(collectMoneyState);
        }
    }
}
