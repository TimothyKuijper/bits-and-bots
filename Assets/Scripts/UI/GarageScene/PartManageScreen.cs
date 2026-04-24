using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Part;

public class PartManageScreen : BaseMenu
{
    private RobotBuilder _robotBuilder;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI partLabel;
    [SerializeField] private TextMeshProUGUI rarityLabel;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI damageLabel;

    [SerializeField] private Slider hpBar;
    [SerializeField] private SegmentSlider sliderSegments;
    [SerializeField] private TextMeshProUGUI deductLabel;
    [SerializeField] private TextMeshProUGUI addLabel;

    [SerializeField] private Button backButton;

    [Header("Purchase Button")]
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI purchaseLabel;

    [Header("Display")]
    [SerializeField] private List<Sprite> partSprites;
    [SerializeField] private Color sameColor = Color.black;
    [SerializeField] private Color minusColor = Color.red;
    [SerializeField] private Color plusColor = Color.green;

    private void Start()
    {
        backButton.onClick.AddListener(HideMenu);
    }

    public void ShowPart(PartType partType)
    {
        var savedPart = LoadPart(partType);

        partLabel.text = savedPart.PartName;
        rarityLabel.text = "Rarity: " + savedPart.Rarity;
        iconImage.sprite = partSprites[(int)partType];
        iconImage.color = ColorTTools.GetColorFromHex(savedPart.PartColor);

        var damage = savedPart.Value.ToString();
        damageLabel.color = sameColor;
        damageLabel.text = "Damage: " + damage;

        hpBar.maxValue = savedPart.MaxPartHealth;
        hpBar.value = savedPart.PartHealth;
        addLabel.text = "";
        deductLabel.text = "";
        sliderSegments.SetSegments();

        ShowMenu();

        purchaseButton.onClick.RemoveAllListeners();
        var cost = 10; // CALCULATE COST WITH SCALING AND RARITY

        if (savedPart.IsDamaged == false)
        {
            purchaseButton.gameObject.SetActive(false);
            return;
        }
        purchaseButton.gameObject.SetActive(true);

        if (MoneyBag.HasEnough(cost) == false)
        {
            purchaseLabel.text = "Too poor! - $ " + cost.ToString();
            purchaseButton.interactable = false;
            return;
        }

        purchaseLabel.text = "Repair - $ " + cost.ToString();
        purchaseButton.interactable = true;
        purchaseButton.onClick.AddListener(() =>
        {
            savedPart.PartHealth = savedPart.MaxPartHealth;
            _robotBuilder.ChangeRobotPart(savedPart);
            MoneyBag.Remove(cost);
            HideMenu();
        });
    }

    public void ShowComparePart(Part comparePart)
    {
        var partType = comparePart.Type;
        var savedPart = LoadPart(partType);

        partLabel.text = comparePart.PartName;
        rarityLabel.text = "Rarity: " + comparePart.Rarity;
        iconImage.sprite = partSprites[(int)partType];
        iconImage.color = ColorTTools.GetColorFromHex(comparePart.PartColor);

        var damage = comparePart.Value.ToString();
        if (comparePart.Value != savedPart.Value)
        {
            var damageDifference = (comparePart.Value - savedPart.Value).ToString();
            var isAbove = comparePart.Value > savedPart.Value;

            damage = isAbove ? "+" + damageDifference : damageDifference;
            damageLabel.color = isAbove ? plusColor : minusColor;
        }
        else damageLabel.color = sameColor;
        damageLabel.text = "Damage: " + damage;

        if (comparePart.MaxPartHealth != savedPart.MaxPartHealth)
        {
            if (comparePart.PartHealth < savedPart.PartHealth)
            {
                hpBar.maxValue = savedPart.PartHealth;
                hpBar.value = comparePart.PartHealth;
                deductLabel.text = "-" + (savedPart.PartHealth - comparePart.PartHealth).ToString();
                addLabel.text = "";
            }
            else
            {
                hpBar.maxValue = comparePart.MaxPartHealth;
                hpBar.value = comparePart.PartHealth;
                deductLabel.text = "";
                addLabel.text = "+" + (comparePart.PartHealth - savedPart.PartHealth).ToString();
            }
        }
        else
        {
            hpBar.maxValue = comparePart.MaxPartHealth;
            hpBar.value = comparePart.PartHealth;
            addLabel.text = "";
            deductLabel.text = "";
        }
        sliderSegments.SetSegments();

        ShowMenu();

        purchaseButton.gameObject.SetActive(true);
        purchaseButton.onClick.RemoveAllListeners();
        var cost = 25; // CALCULATE COST WITH SCALING AND RARITY

        if (MoneyBag.HasEnough(cost) == false)
        {
            purchaseLabel.text = "Too poor! - $ " + cost.ToString();
            purchaseButton.interactable = false;
            return;
        }

        purchaseLabel.text = "Buy - $ " + cost.ToString(); 
        purchaseButton.interactable = true;
        purchaseButton.onClick.AddListener(() =>
        {
            _robotBuilder.ChangeRobotPart(comparePart);
            MoneyBag.Remove(cost);
            HideMenu();
        });
    }

    private Part LoadPart(PartType partType)
    {
        if (_robotBuilder == null) _robotBuilder = FindObjectsByType<RobotBuilder>(FindObjectsSortMode.None)[0];
        var robot = _robotBuilder.LoadRobot();
        switch (partType)
        {
            case PartType.Body: return robot.Body;
            case PartType.Weapon: return robot.Weapon;
            case PartType.Cpu: return robot.Cpu;
            case PartType.MovementModule: return robot.MovementModule;
        }
        return robot.Head;
    }
}
