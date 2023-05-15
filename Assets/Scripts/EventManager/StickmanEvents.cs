using System;
using UnityEngine;


public static class StickmanEvents
{
    public static Action<GameObject> OnMoneyCollected;
    public static Action<GameObject> OnMoneyDeposited;
    public static Action<GameObject> OnReachedCounter;
    public static Action<GameObject> OnReachedDepositPosition;
    public static Action<GameObject> OnCollecting;
}



