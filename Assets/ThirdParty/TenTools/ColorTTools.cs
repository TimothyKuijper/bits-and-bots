using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ColorTTools
{
    // Got tired of Unity lacking color functions i can easily reuse for UI and stuff :)

    public static string GetHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGBA(color);
    }

    public static Color GetColorFromHex(Color color)
    {
        ColorUtility.TryParseHtmlString("#" + color, out var newColor);
        return newColor;
    }

    public static Color GetFadeColor(Image image, bool faded)
    {
        var color = image.color;
        return new Color(color.r, color.g, color.b, faded ? 0 : 1);
    }

    public static Color GetFadeColor(TextMeshProUGUI text, bool faded)
    {
        var color = text.color;
        return new Color(color.r, color.g, color.b, faded ? 0 : 1);
    }
}
