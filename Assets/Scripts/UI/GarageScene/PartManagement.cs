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
    [SerializeField] private TileBar tileBar;
    [SerializeField] private TileBar tileBarCompare;
    [SerializeField] private TileBar tileBarBack;
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

    public void ShowPart(PartType partType, bool pullNewPart = false)
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
            var damageComparison = (part.Value - savedPart.Value).ToString();
            var isAbove = part.Value > savedPart.Value;

            damage = isAbove ? "+" + damageComparison : damageComparison;
            damageLabel.color = isAbove ? plusColor : minusColor;
        }
        else damageLabel.color = sameColor;
        damageLabel.text = "Damage: " + damage;

        tileBar.Value = part.PartHealth;
        tileBarCompare.Value = savedPart.PartHealth;
        tileBarBack.Value = part.MaxPartHealth;

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
