using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartUtility
{
    private const float RarityDivider = 2f; 
    private const float ValueScale = 0.01f; 
    public static Part ScalePart(Part part, float scaling = 0)
    {
        var rarity = (int)Mathf.Floor(part.Rarity *  scaling / RarityDivider);
        var value = part.Value * scaling * ValueScale;
        return new Part(part.Type, rarity, value, part.BreakModifier, part.MaxPartHealth, part.PartHealth);
    }
}
