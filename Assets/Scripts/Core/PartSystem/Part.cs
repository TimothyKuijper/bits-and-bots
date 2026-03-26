using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Robot Part", menuName = "Bits & Bots/Parts/New Part")]
public class Part : ScriptableObject
{
    public enum PartType
    {
        Head,
        Weapon,
        Body,
        MovementModule,
        Cpu
    }
    [Tooltip("The type of the part. Decides which slot this part is used in")] public PartType Type = PartType.Head;
    [Tooltip("The rarity of the part, used to scale enemies to this part")] public int Rarity;
    [Tooltip("Value used for calculations with the part in moment-to-moment gameplay")] public float Value;
    [Tooltip("The mod that gets used over the value when calculating part value")] public float breakMod;
    public float GetPartValue() => _isBroken ? Value * breakMod : Value;


    [Tooltip("Health of the part and how many damage it can take till broken")][SerializeField] private int partHealth;
    public int PartHealth
    {
        get => partHealth;
        set
        {
            partHealth = value;
            _isBroken = partHealth <= 0;
        }
    }
    private bool _isBroken;
    public bool IsBroken => _isBroken;
}
