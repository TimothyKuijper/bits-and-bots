using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yakanashe.Yautl;

[RequireComponent(typeof(CanvasGroup))]
public class BaseMenu : MonoBehaviour
{
    [SerializeField] private bool startInactive;

    [Header("Menu Flavor")]
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private EaseType fadeEase = EaseType.Linear;
    [SerializeField] private List<Graphic> ignoredGraphics;

    [Header("Base Events")]
    [SerializeField] public UnityEvent<bool> fadeComplete = new();
    [SerializeField] public UnityEvent onShow = new();
    [SerializeField] public UnityEvent onHide = new();

    private CanvasGroup _canvasGroup;
    private bool _isTransitioning = false;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.interactable = !startInactive;
        _canvasGroup.interactable = !startInactive;
        _canvasGroup.alpha = startInactive ? 0 : 1;

        fadeComplete.AddListener((value) => _canvasGroup.interactable = !value);
        fadeComplete.AddListener((value) => _canvasGroup.blocksRaycasts = !value);
    }

    public void ShowMenu()
    {
        if (_isTransitioning) return;

        StopAllCoroutines();
        onShow.Invoke();

        StartCoroutine(FadeMenu(false));
    }

    public void HideMenu()
    {
        if (_isTransitioning) return;

        StopAllCoroutines();
        onHide.Invoke();

        StartCoroutine(FadeMenu(true));
    }

    private IEnumerator FadeMenu(bool fadeOut)
    {
        _canvasGroup.alpha = fadeOut ? 1 : 0;
        _isTransitioning = true;
        yield return new WaitForEndOfFrame();
        _canvasGroup.FadeTo(fadeOut ? 0 : 1, fadeTime, EaseType.InOutSine);
        yield return new WaitForSeconds(fadeTime);
        fadeComplete.Invoke(fadeOut);
        _isTransitioning = false;
        yield break;
    }
}
