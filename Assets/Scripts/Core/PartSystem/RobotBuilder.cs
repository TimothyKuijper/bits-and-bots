using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yakapedia;

public class RobotBuilder : MonoBehaviour
{
    [SerializeField] private Part defaultHead;
    [SerializeField] private Part defaultWeapon;
    [SerializeField] private Part defaultBody;
    [SerializeField] private Part defaultCpu;
    [SerializeField] private Part defaultMovement;

    private const string HeadKey = "Head"; 
    private const string WeaponKey = "Weapon"; 
    private const string BodyKey = "Body"; 
    private const string CpuKey = "Cpu"; 
    private const string MovementKey = "Movement";
    

    public Robot LoadRobot()
    {
        var robot = new Robot();

        var head = PersistentData.GetSerialized(HeadKey, Instantiate(defaultHead));
        var weapon = PersistentData.GetSerialized(WeaponKey, Instantiate(defaultWeapon));
        var body = PersistentData.GetSerialized(BodyKey, Instantiate(defaultBody));
        var cpu = PersistentData.GetSerialized(CpuKey, Instantiate(defaultCpu));
        var movement = PersistentData.GetSerialized(MovementKey, Instantiate(defaultMovement));

        robot.Head = head;
        robot.Weapon = weapon;
        robot.Body = body;
        robot.Cpu = cpu;
        robot.MovementModule = movement;

        return robot;
    }

    public void ChangeRobotPart(Part newPart)
    {
        switch (newPart.Type)
        {
            case Part.PartType.Head: PersistentData.Set(HeadKey, newPart); break;
            case Part.PartType.Weapon: PersistentData.Set(WeaponKey, newPart); break;
            case Part.PartType.Body: PersistentData.Set(BodyKey, newPart); break;
            case Part.PartType.Cpu: PersistentData.Set(CpuKey, newPart); break;
            case Part.PartType.MovementModule: PersistentData.Set(MovementKey, newPart); break;
        }
    }
}
