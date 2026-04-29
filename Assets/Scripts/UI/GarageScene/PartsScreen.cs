using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yakanashe.Yautl;

public class PartsScreen : BaseMenu
{
    [Header("Part Buttons")]
    [SerializeField] private List<PartButton> partButtons = new();

    [Header("Backpanel")]
    [SerializeField] private Image backPanel;
    [SerializeField] private Color backColor;
    [SerializeField] private float backFadeTime = 1;
    [SerializeField] private EaseType backEaseType = EaseType.Linear;

    [Header("Money")]
    [SerializeField] private TextMeshProUGUI moneyLabel;

    [Header("Back Button")]
    [SerializeField] private Button backButton;
    [SerializeField] private Image backButtonIcon;
    [SerializeField] private Color buttonColor = Color.white;
    [SerializeField] private Color iconColor = Color.white;
    [SerializeField] private float buttonFadeTime = .1f;
    [SerializeField] private EaseType buttonEaseType = EaseType.Linear;

    [Header("Part Display")]
    [SerializeField] private PartManageScreen partDisplay;

    private void Start()
    {
        foreach (var button in partButtons) button.SetPartMenu(this);

        partDisplay.onHide.AddListener(() => UpdateBackPanel(false));
        partDisplay.onHide.AddListener(() => UpdateBackButton(true));
        partDisplay.onHide.AddListener(() => UpdateMoney());
        partDisplay.onShow.AddListener(() => UpdateMoney());

        onHide.AddListener(() => SetPartButtons(false));
        onShow.AddListener(() => SetPartButtons(true));
        onShow.AddListener(() => UpdateMoney());

        UpdateMoney();
    }

    public void OpenPart(Part.PartType partType)
    {
        partDisplay.ShowPart(partType);
        UpdateBackPanel(true);
        UpdateBackButton(false);
    }

    public void ComparePart(Part part)
    {
        partDisplay.ShowComparePart(part);
        UpdateBackPanel(true);
        UpdateBackButton(false);
    }

    public void UpdateBackPanel(bool show)
    {
        backPanel.gameObject.SetActive(true);
        backPanel.ColorTo(show ? backColor : Color.clear, backFadeTime, backEaseType).OnComplete(() => backPanel.gameObject.SetActive(show));
    }

    public void UpdateBackButton(bool show)
    {
        backButton.gameObject.SetActive(true);

        var button = show ? buttonColor : Color.clear;
        var icon = show ? iconColor : Color.clear;
        backButton.image.ColorTo(button, buttonFadeTime, buttonEaseType).OnComplete(() => backButton.gameObject.SetActive(show));
        backButtonIcon.ColorTo(icon, buttonFadeTime, buttonEaseType);
    }

    public void SetPartButtons(bool active)
    {
        foreach (var button in partButtons) button.canBePressed = active;
    }

    public void UpdateMoney() => moneyLabel.text = "$ " + MoneyBag.CurrentMoney.ToString();
}
