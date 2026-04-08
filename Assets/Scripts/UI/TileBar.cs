using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class TileBar : MonoBehaviour
{
    enum FillMethod
    {
        HorizontalCenter,
        HorizontalRight,
        HorizontalLeft,
        VerticalCenter,
        VerticalUp,
        VerticalDown
    }

    [SerializeField] private  FillMethod fillMethod = FillMethod.HorizontalRight;
    [SerializeField] private Texture progressTexture;
    [SerializeField] private float maxValue = 100;
    [SerializeField] private float progressValue = 0;
    public float Value
    {
        get => progressValue;
        set => UpdateBar(value);
    }

    private void OnValidate() => UpdateBar(progressValue);

    public void UpdateBar(float value)
    {
        progressValue = Mathf.Clamp(value, 0, maxValue);

        var horizontal = fillMethod == FillMethod.HorizontalCenter || fillMethod == FillMethod.HorizontalRight || fillMethod == FillMethod.HorizontalLeft;

        var rawImage = GetComponent<RawImage>();
        var uvRect = horizontal ? new Rect(0, 0, progressValue, 0) : new Rect(0, 0, 0, progressValue);
        rawImage.texture = progressTexture;
        rawImage.uvRect = uvRect;

        var rectTransform = GetComponent<RectTransform>();
        switch (fillMethod)
        {
            case FillMethod.HorizontalCenter: rectTransform.pivot = Vector2.one / 2; break;
            case FillMethod.HorizontalRight: rectTransform.pivot = Vector2.zero; break;
            case FillMethod.HorizontalLeft: rectTransform.pivot = Vector2.one; break;
            case FillMethod.VerticalCenter: rectTransform.pivot = Vector2.one / 2; break;
            case FillMethod.VerticalUp: rectTransform.pivot = Vector2.zero; break;
            case FillMethod.VerticalDown: rectTransform.pivot = Vector2.up; break;
        }
        var newSize = horizontal ? new Vector2(progressValue * progressTexture.width, progressTexture.height) : new Vector2(progressTexture.width, progressValue * progressTexture.height);
        rectTransform.sizeDelta = newSize;
    }
}
