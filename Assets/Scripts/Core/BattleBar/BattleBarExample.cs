using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBarExample : MonoBehaviour
{
    public BattleBar bar;
    public ParticleSystem PlayerParticles;
    public ParticleSystem EnemyParticles;

    private void Start()
    {
        bar.OnBarChanged.AddListener((type, amount) =>
        {
            if (type == EntityType.Player)
            {
                PlayerParticles.Play();
                return;
            }
            
            EnemyParticles.Play();
        });
        bar.OnBarFull.AddListener((type) => { Debug.Log("bar filled!"); });
    }

    public void AddToPlayer()
    {
        bar.Add(EntityType.Player, 5f);
    }

    public void AddToEnemy()
    {
        bar.Add(EntityType.Enemy, 5f);
    }
}