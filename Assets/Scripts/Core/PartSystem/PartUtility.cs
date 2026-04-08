using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yakapedia;

public class PartUtility
{
    private const float ScalingScale = 0.01f; 
    public static Part ScalePart(Part part, float scaling = 1)
    {
        var modifiedRarity = Mathf.Clamp(part.Rarity + (part.Rarity * scaling * ScalingScale) - part.Rarity, part.Rarity, Mathf.Infinity);
        var rarity = (int)Mathf.Floor(modifiedRarity);
        var value = (part.Value + (part.Value * scaling * ScalingScale)).RoundToNearest(0.1f);
        return new Part(part.PartName, part.PartColor, part.Type, rarity, value, part.BreakModifier, part.MaxPartHealth, part.PartHealth);
    }
}
