using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBarExample : MonoBehaviour
{
    [SerializeField] private BattleBar bar;
    [SerializeField] private IconSwitcher iconSwitcher;

    private void Start()
    {
        iconSwitcher.onMatchMade.AddListener(AddToPlayer);
    }

    public void AddToPlayer(int amount)
    {
        bar.Add(EntityType.Player, amount);
    }

    public void AddToEnemy(float amount = 5f)
    {
        bar.Add(EntityType.Enemy, amount);
    }
}