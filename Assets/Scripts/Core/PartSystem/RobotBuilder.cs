using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yakapedia;

public class RobotBuilder : MonoBehaviour
{
    [SerializeField] private Part DefaultHead;
    [SerializeField] private Part DefaultWeapon;
    [SerializeField] private Part DefaultBody;
    [SerializeField] private Part DefaultCpu;
    [SerializeField] private Part DefaultMovement;

    private const string HeadKey = "Head"; 
    private const string WeaponKey = "Weapon"; 
    private const string BodyKey = "Body"; 
    private const string CpuKey = "Cpu"; 
    private const string MovementKey = "Movement";
    

    public Robot LoadRobot()
    {
        var robot = new Robot();

        var head = PersistentData.GetSerialized(HeadKey, Instantiate(DefaultHead));
        var weapon = PersistentData.GetSerialized(WeaponKey, Instantiate(DefaultWeapon));
        var body = PersistentData.GetSerialized(BodyKey, Instantiate(DefaultBody));
        var cpu = PersistentData.GetSerialized(CpuKey, Instantiate(DefaultCpu));
        var movement = PersistentData.GetSerialized(MovementKey, Instantiate(DefaultMovement));

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
