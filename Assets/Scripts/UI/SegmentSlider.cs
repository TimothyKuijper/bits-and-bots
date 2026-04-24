using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Slider))]
public class SegmentSlider : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private Slider _slider;
    private RectTransform _rectTransform;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener((arg) => SetSegments());
        _rectTransform = GetComponent<RectTransform>();

        SetSegments();
    }

    public void SetSegments()
    {
        if (_slider == null) return;
        var length = fillImage.sprite.rect.width / _rectTransform.rect.width * (_slider.maxValue / 2);
        fillImage.pixelsPerUnitMultiplier = length;
    }
}
