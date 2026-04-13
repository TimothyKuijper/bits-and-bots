using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TickSystem/TickPattern")]
public class TickPattern : ScriptableObject
{
    [SerializeField] public List<Tick> Ticks = new();
}