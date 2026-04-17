using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Part;

public class PartManagement : BaseMenu
{
    [Header("Part System")]
    [SerializeField] private RobotBuilder robotBuilder;
    [SerializeField] private PartCollection partCollection;
    // OBJECT THAT KEEPS TRACK OF SCALING

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI partLabel;
    [SerializeField] private TextMeshProUGUI rarityLabel;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI damageLabel;

    //[SerializeField] private TileBar tileBar;
    //[SerializeField] private TileBar tileBarCompare;
    //[SerializeField] private TileBar tileBarBack;
    //[SerializeField] private TextMeshProUGUI hpComparison;
    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI deductLabel;
    [SerializeField] private TextMeshProUGUI addLabel;

    [SerializeField] private Button backButton;

    [Header("Display")]
    [SerializeField] private List<Sprite> partSprites;
    [SerializeField] private Color sameColor = Color.black;
    [SerializeField] private Color minusColor = Color.red;
    [SerializeField] private Color plusColor = Color.green;

    private void Start()
    {
        backButton.onClick.AddListener(HideMenu);
    }

    public void ShowPart(PartType partType, bool pullNewPart = true)
    {
        var savedPart = LoadPart(partType);
        var part = pullNewPart ? PartUtility.ScalePart(partCollection.GetRandomPartByType(partType)) : savedPart;// INPUT SCALING FOR PART HERE

        partLabel.text = part.PartName;
        rarityLabel.text = "Rarity: " + part.Rarity;
        iconImage.sprite = partSprites[(int)partType];
        iconImage.color = ColorTTools.GetColorFromHex(part.PartColor);

        var damage = part.Value.ToString();
        if (pullNewPart && part.Value != savedPart.Value)
        {
            var damageDifference = (part.Value - savedPart.Value).ToString();
            var isAbove = part.Value > savedPart.Value;

            damage = isAbove ? "+" + damageDifference : damageDifference;
            damageLabel.color = isAbove ? plusColor : minusColor;
        }
        else damageLabel.color = sameColor;
        damageLabel.text = "Damage: " + damage;

        //tileBar.Value = part.PartHealth;
        //tileBarCompare.Value = savedPart.PartHealth;
        //tileBarBack.Value = part.MaxPartHealth;

        //var hpDifference = savedPart.PartHealth - part.PartHealth;
        //hpComparison.text = savedPart.PartHealth > part.PartHealth ? "-" + hpDifference.ToString() : "";
        if (pullNewPart && part.MaxPartHealth != savedPart.MaxPartHealth)
        {
            if (part.PartHealth < savedPart.PartHealth)
            {
                hpBar.maxValue = savedPart.PartHealth;
                hpBar.value = part.PartHealth;
                deductLabel.text = "-" + (savedPart.PartHealth - part.PartHealth).ToString();
                addLabel.text = "";
            }
            else
            {
                hpBar.maxValue = part.MaxPartHealth;
                hpBar.value = part.PartHealth;
                deductLabel.text = "";
                addLabel.text = "+" + (part.PartHealth - savedPart.PartHealth).ToString();
            }
        }
        else
        {
            hpBar.maxValue = part.MaxPartHealth;
            hpBar.value = part.PartHealth;
            addLabel.text = "";
            deductLabel.text = "";
        }

        ShowMenu();
    }

    private Part LoadPart(PartType partType)
    {
        var robot = robotBuilder.LoadRobot();
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
