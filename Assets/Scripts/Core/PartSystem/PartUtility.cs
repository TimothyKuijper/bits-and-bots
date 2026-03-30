using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartUtility
{
    private const float ScalingScale = 0.01f; 
    public static Part ScalePart(Part part, float scaling = 1)
    {
        var rarityModifier = 1 + (scaling * ScalingScale);
        var rarity = (int)Mathf.Floor(part.Rarity * rarityModifier);
        var value = part.Value + (part.Value * scaling * ScalingScale);
        return new Part(part.PartName, ColorUtility.ToHtmlStringRGB(part.PartColor), part.Type, rarity, value, part.BreakModifier, part.MaxPartHealth, part.PartHealth);
    }
}
