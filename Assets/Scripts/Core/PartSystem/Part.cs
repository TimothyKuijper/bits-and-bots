using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Robot Part", menuName = "Bits & Bots/Parts/New Part")]
[Serializable]
public class Part : ScriptableObject, IConvertible
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
    [Tooltip("Color of part, in HEX")] public string PartColor = "ffffff";

    [Header("Settings")]
    [Tooltip("The type of the part. Decides which slot this part is used in")] public PartType Type;
    [Tooltip("The rarity of the part, used to scale enemies to this part")][Range(1, 10)] public int Rarity;
    [Tooltip("Value used for calculations with the part in moment-to-moment gameplay")][Range(1f, 100f)] public float Value;
    [Tooltip("The mod that gets used over the value when calculating part value")][Range(0.01f, 2f)] public float BreakModifier;
    public float GetPartValue() => IsBroken ? Value * BreakModifier : Value;

    [Tooltip("Max health of the part and how many damage it can take till broken")][SerializeField][Range(1, 10)] public int MaxPartHealth;
    [Tooltip("Current health of a part")][SerializeField][Range(1, 10)] private int CurrentPartHealth;
    public int PartHealth
    {
        get => CurrentPartHealth;
        set
        {
            CurrentPartHealth = value >= MaxPartHealth ? MaxPartHealth : value;
        }
    }
    public bool IsDamaged => CurrentPartHealth < MaxPartHealth;
    public bool IsBroken => CurrentPartHealth <= 0;


    public Part(string name = "Basic Part", string colorString = "ffffff", PartType type = PartType.Head, int rarity = 0, float value = 0, float breakMod = 0.5f, int maxPartHealth = 3, int currentPartHealth = 3)
    {
        PartName = name;
        PartColor = colorString;

        Type = type;
        Rarity = rarity;
        Value = value;
        BreakModifier = breakMod;
        MaxPartHealth = maxPartHealth;
        PartHealth = currentPartHealth;
    }

    public Part(Part examplePart)
    {
        PartName = examplePart.PartName;
        PartColor = examplePart.PartColor;

        Type = examplePart.Type;
        Rarity = examplePart.Rarity;
        Value = examplePart.Value;
        BreakModifier = examplePart.BreakModifier;
        MaxPartHealth = examplePart.MaxPartHealth;
        PartHealth = examplePart.PartHealth;
    }


    TypeCode IConvertible.GetTypeCode()
    {
        return TypeCode.String;
    }
    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    byte IConvertible.ToByte(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    char IConvertible.ToChar(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    double IConvertible.ToDouble(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    short IConvertible.ToInt16(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    int IConvertible.ToInt32(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    long IConvertible.ToInt64(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    float IConvertible.ToSingle(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    string IConvertible.ToString(IFormatProvider provider)
    {
        var stringObject = JsonUtility.ToJson(this);
        return stringObject;
    }
    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
        var stringObject = JsonUtility.ToJson(this);
        return stringObject;
    }
    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
}
