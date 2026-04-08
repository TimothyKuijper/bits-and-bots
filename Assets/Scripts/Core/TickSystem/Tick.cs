using System;
using UnityEngine;

[Serializable]
public class Tick
{
    [Range(1, 20)] public float Damage;
    [Range(0.1f, 5f)] public float Duration;
}