using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yakanashe.Yautl;

public class PartsScreen : BaseMenu
{
    [Header("Backpanel")]
    [SerializeField] private Image backPanel;
    [SerializeField] private Color backColor;
    [SerializeField] private float backFadeTime = 1;
    [SerializeField] private EaseType backEaseType = EaseType.Linear;

    [Header("Part Display")]
    [SerializeField] private PartDisplay partDisplay;

    private void Start()
    {
        onHide.AddListener(() => partDisplay.gameObject.SetActive(false));
        partDisplay.onHide.AddListener(() => UpdateBackPanel(false));
    }

    public void OpenPart(Part.PartType partType)
    {
        partDisplay.ShowPart(partType);
        UpdateBackPanel(true);
    }

    public void UpdateBackPanel(bool show)
    {
        backPanel.gameObject.SetActive(true);
        backPanel.ColorTo(show ? backColor : Color.clear, backFadeTime, backEaseType).OnComplete(() => backPanel.gameObject.SetActive(show));
    }
}
