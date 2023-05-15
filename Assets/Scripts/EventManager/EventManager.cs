using System;

public static class EventManager
{
    //Gameplay Events
    public static event Action ScreenClicked;
    public static event Action MoneyDeposited;
    public static event Action MoneyAmountUpdated;

    //Upgrades
    public static event Action WorkerUpgradePressed;
    public static event Action MoneyCounterUpgradePressed;
    public static event Action VaultSizeUpgradePressed;
    public static event Action MergeWorkerUpgradePressed;
    public static event Action MergeCounterUpgradePressed;

    public static event Action WorkerMaxUpgradeReached;
    public static event Action MoneyCounterMaxUpgradeReached;
    public static event Action VaultMaxUpgradeReached;
    public static event Action MergeWorkerMaxUpgradeReached;
    public static event Action MergeCounterMaxUpgradeReached;

    public static void ScreenClickedEvent()
    {
        ScreenClicked?.Invoke();
    }

    public static void MoneyDepositedEvent()
    {
        MoneyDeposited?.Invoke();
    }

    public static void MoneyAmountUpdatedEvent()
    {
        MoneyAmountUpdated?.Invoke();
    }

    public static void WorkerUpgradePressedEvent()
    {
        WorkerUpgradePressed?.Invoke();
    }

    public static void MoneyCounterUpgradePressedEvent()
    {
        MoneyCounterUpgradePressed?.Invoke();
    }

    public static void VaultSizeUpgradePressedEvent()
    {
        VaultSizeUpgradePressed?.Invoke();
    }

    public static void MergeWorkerUpgradePressedEvent()
    {
        MergeWorkerUpgradePressed?.Invoke();
    }

    public static void MergeCounterUpgradePressedEvent()
    {
        MergeCounterUpgradePressed?.Invoke();
    }

    public static void WorkerMaxUpgradeReachedEvent()
    {
        WorkerMaxUpgradeReached?.Invoke();
    }

    public static void MoneyCounterMaxUpgradeReachedEvent()
    {
        MoneyCounterMaxUpgradeReached?.Invoke();
    }

    public static void VaultMaxUpgradeReachedEvent()
    {
        VaultMaxUpgradeReached?.Invoke();
    }

    public static void MergeWorkerMaxUpgradeReachedEvent()
    {
        MergeWorkerMaxUpgradeReached?.Invoke();
    }

    public static void MergeCounterMaxUpgradeReachedEvent()
    {
        MergeCounterMaxUpgradeReached?.Invoke();
    }
}