using System.Collections;
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

    [Header("Base Events")]
    [SerializeField] private UnityEvent<bool> fadeComplete = new();
    [SerializeField] private UnityEvent onShow = new();
    [SerializeField] private UnityEvent onHide = new();

    private Image[] _uiImages;
    private TextMeshProUGUI[] _text;

    private void Awake()
    {
        _uiImages = GetComponentsInChildren<Image>(true);
        _text = GetComponentsInChildren<TextMeshProUGUI>(true);
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
        foreach (var image in _uiImages) image.color = ColorTTools.GetFadeColor(image, faded);
        foreach (var label in _text) label.color = ColorTTools.GetFadeColor(label, faded);
    }

    private IEnumerator FadeMenu(bool fadeOut)
    {
        SetMenuColor(!fadeOut);
        foreach (var image in _uiImages) image.ColorTo(ColorTTools.GetFadeColor(image, fadeOut), fadeTime, fadeEase);
        foreach (var label in _text) label.ColorTo(ColorTTools.GetFadeColor(label, fadeOut), fadeTime, fadeEase);

        yield return new WaitForSeconds(fadeTime);
        fadeComplete.Invoke(fadeOut);
        yield break;
    }
}
