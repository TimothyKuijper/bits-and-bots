using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartDisplay : BaseMenu
{
    [SerializeField] private RobotBuilder robotBuilder;
    [SerializeField] private TextMeshProUGUI partLabel;
    [SerializeField] private Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(HideMenu);
    }

    public void ShowPart(Part.PartType partType)
    {
        ShowMenu();

        var robot = robotBuilder.LoadRobot();
        var part = robot.Head;
        switch (partType)
        {
            case Part.PartType.Head: break;
            case Part.PartType.Body: part = robot.Body; break;
            case Part.PartType.Weapon: part = robot.Weapon; break;
            case Part.PartType.Cpu: part = robot.Cpu; break;
            case Part.PartType.MovementModule: part = robot.MovementModule; break;
        }

        //partLabel.text = part.PartName;
    }
}
