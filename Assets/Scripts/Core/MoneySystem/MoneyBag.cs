using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yakapedia;

public class MoneyBag
{
    public Event MoneyUpdated;

    private const string MoneyKey = "Money";
    private const int BaseMoneyAmount = 100;

    public static void AddMoney(int amount) => PersistentData.Set(MoneyKey, CurrentMoney + amount);

    public static int CurrentMoney => PersistentData.Get(MoneyKey, BaseMoneyAmount);

    public static bool HasEnoughMoney(int checkSum)
    {
        return CurrentMoney >= checkSum;
    }

    public static void RemoveMoney(int amount) => PersistentData.Set(MoneyKey, CurrentMoney - amount);
}
