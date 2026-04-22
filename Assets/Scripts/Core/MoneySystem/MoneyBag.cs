using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yakapedia;

public class MoneyBag
{
    public Event MoneyUpdated;

    private const string MoneyKey = "Money";
    private const int BaseMoneyAmount = 100;

    static public void AddMoney(int amount) => PersistentData.Set(MoneyKey, CurrentMoney + amount);

    static public int CurrentMoney => PersistentData.Get(MoneyKey, BaseMoneyAmount);

    static public bool HasEnoughMoney(int checkSum)
    {
        return CurrentMoney >= checkSum;
    }

    static public void RemoveMoney(int amount) => PersistentData.Set(MoneyKey, CurrentMoney - amount);
}
