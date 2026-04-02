using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private BattleBar battleBar;
    [SerializeField] private TickRunner tickRunner;

    private void Start()
    {
        tickRunner.OnAttack.AddListener(damage => { battleBar.Add(EntityType.Enemy, damage); });
        tickRunner.Run();
    }

    public void Stop()
    {
        tickRunner.Stop();
    }
}