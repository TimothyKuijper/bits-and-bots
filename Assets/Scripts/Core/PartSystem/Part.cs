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

    [Header("Flavor")]
    [Tooltip("Name of the part")] public string PartName = "Basic Part";
    [Tooltip("Color of part")] public Color PartColor = Color.white;

    [Header("Settings")]
    [Tooltip("The type of the part. Decides which slot this part is used in")] public PartType Type;
    [Tooltip("The rarity of the part, used to scale enemies to this part")] public int Rarity;
    [Tooltip("Value used for calculations with the part in moment-to-moment gameplay")] public float Value;
    [Tooltip("The mod that gets used over the value when calculating part value")] public float BreakModifier;
    public float GetPartValue() => _isBroken ? Value * BreakModifier : Value;


    [Tooltip("Max health of the part and how many damage it can take till broken")][SerializeField] public int MaxPartHealth;
    [Tooltip("Current health of a part")][SerializeField] private int CurrentPartHealth;
    public int PartHealth
    {
        get => CurrentPartHealth;
        set
        {
            if (value >= MaxPartHealth)
            {
                CurrentPartHealth = MaxPartHealth;
                _isBroken = false;
                return;
            }

            CurrentPartHealth = value;
            _isBroken = CurrentPartHealth <= 0;
        }
    }
    private bool _isBroken;
    public bool IsBroken => _isBroken;


    public Part(string name = "Basic Part", string colorString = "ffffff", PartType type = PartType.Head, int rarity = 0, float value = 0, float breakMod = 0.5f, int maxPartHealth = 3, int currentPartHealth = 3)
    {
        ColorUtility.TryParseHtmlString("#" + colorString, out var newColor);
        PartName = name;
        PartColor = newColor;

        Type = type;
        Rarity = rarity;
        Value = value;
        BreakModifier = breakMod;
        MaxPartHealth = maxPartHealth;
        PartHealth = currentPartHealth;
    }
}
