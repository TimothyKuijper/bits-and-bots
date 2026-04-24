using UnityEngine;
using UnityEngine.UI;

public class ColorTTools
{
    // Got tired of Unity lacking color functions i can easily reuse for UI and stuff :)

    public static string GetHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGBA(color);
    }

    public static Color GetColorFromHex(string color)
    {
        ColorUtility.TryParseHtmlString("#" + color, out var newColor);
        return newColor;
    }

    public static Color GetFadeColor(Graphic graphic, bool faded)
    {
        var color = graphic.color;
        return new Color(color.r, color.g, color.b, faded ? 0 : 1);
    }
}
