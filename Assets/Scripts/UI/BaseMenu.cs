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
    [SerializeField] private UnityEvent<bool> fadeComplete = new();
    [SerializeField] private UnityEvent onShow = new();
    [SerializeField] private UnityEvent onHide = new();

    private Graphic[] _graphics;

    private void Awake()
    {
        _graphics = GetComponentsInChildren<Graphic>(true);
        fadeComplete.AddListener((value) => gameObject.SetActive(!value));
    }

    public void ShowMenu()
    {
        StopAllCoroutines();
        onShow.Invoke();
        gameObject.SetActive(true);

        StartCoroutine(FadeMenu(false));
    }

    public void HideMenu()
    {
        StopAllCoroutines();
        onHide.Invoke();
        gameObject.SetActive(true);

        StartCoroutine(FadeMenu(true));
    }

    private void SetMenuColor(bool faded)
    {
        foreach (var graphic in _graphics) if (ignoredGraphics.Contains(graphic) == false) graphic.color = ColorTTools.GetFadeColor(graphic, faded);
    }

    private IEnumerator FadeMenu(bool fadeOut)
    {
        SetMenuColor(!fadeOut);
        foreach (var graphic in _graphics) if (ignoredGraphics.Contains(graphic) == false) graphic.ColorTo(ColorTTools.GetFadeColor(graphic, fadeOut), fadeTime, fadeEase);

        yield return new WaitForSeconds(fadeTime);
        fadeComplete.Invoke(fadeOut);
        yield break;
    }
}
