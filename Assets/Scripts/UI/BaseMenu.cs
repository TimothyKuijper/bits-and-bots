using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yakanashe.Yautl;

public class BaseMenu : MonoBehaviour
{
    [Header("Menu Flavor")]
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private EaseType fadeEase = EaseType.Linear;
    [SerializeField] private List<Graphic> ignoredGraphics;

    [Header("Base Events")]
    [SerializeField] public UnityEvent<bool> fadeComplete = new();
    [SerializeField] public UnityEvent onShow = new();
    [SerializeField] public UnityEvent onHide = new();

    private Graphic[] _graphics;
    private bool _isTransitioning = false;

    private void Awake()
    {
        _graphics = GetComponentsInChildren<Graphic>(true);
        fadeComplete.AddListener((value) => gameObject.SetActive(!value));
    }

    public void ShowMenu()
    {
        if (_isTransitioning) return;

        StopAllCoroutines();
        onShow.Invoke();
        gameObject.SetActive(true);

        StartCoroutine(FadeMenu(false));
    }

    public void HideMenu()
    {
        if (_isTransitioning) return;

        StopAllCoroutines();
        onHide.Invoke();
        gameObject.SetActive(true);

        StartCoroutine(FadeMenu(true));
    }

    private IEnumerator FadeMenu(bool fadeOut)
    {
        foreach (var graphic in _graphics)
        {
            if (ignoredGraphics.Contains(graphic) == true) continue;
            if (graphic.color == ColorTTools.GetFadeColor(graphic, fadeOut)) graphic.color = ColorTTools.GetFadeColor(graphic, !fadeOut);
        }
        yield return new WaitForEndOfFrame();

        _isTransitioning = true;
        foreach (var graphic in _graphics)
        {
            if (ignoredGraphics.Contains(graphic) == true) continue;
            graphic.ColorTo(ColorTTools.GetFadeColor(graphic, fadeOut), fadeTime, fadeEase);
        }

        yield return new WaitForSeconds(fadeTime);
        fadeComplete.Invoke(fadeOut);
        _isTransitioning = false;
        yield break;
    }
}
