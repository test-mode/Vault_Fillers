using Ambrosia.StateMachine;
using DG.Tweening;
using UnityEngine;

public class CollectMoneyState : State
{
    public StickmanManager stickmanManager;
    public State waitState;

    protected override void OnEnter()
    {
        StickmanEvents.OnMoneyCollected += OnMoneyCollected;



        if (stickmanManager.moneyStack.GetComponent<StackManager>().moneyList.Count > 0)
        {
            stickmanManager.gameObject.transform.DOPunchScale(Vector3.one * 1.5f, 1);
            if (stickmanManager.moneyStack.GetComponent<StackManager>().counter != null)
            {
                stickmanManager.moneyStack.GetComponent<StackManager>().counter.transform.DOPunchScale(Vector3.one * 1.5f, 1);
            }
            

            StartCoroutine(stickmanManager.moneyStack.GetComponent<StackManager>().CollectMoney());
        }
        else
        {
            StateMachine.TransitionTo(waitState);
        }

        
    }

    protected override void OnExit()
    {
        StickmanEvents.OnMoneyCollected -= OnMoneyCollected;
    }

    private void Update()
    {

    }

    void OnMoneyCollected(GameObject stack)
    {
        if (stickmanManager.moneyStack == stack)
        {
            stickmanManager.spawnFinished = false;
            StateMachine.TransitionToNextState();
        }
    }
}