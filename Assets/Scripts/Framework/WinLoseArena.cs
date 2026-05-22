using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yakanashe.Wiper;
using Yakanashe.Yautl;
using EaseType = Yakanashe.Yautl.EaseType;

public class WinLoseArena : MonoBehaviour
{
    public Transform WinScreen, LoseScreen;
    public GameObject SelectUI;
    public Image Backdrop;
    public BattleBar BattleBar;
    public Transition Transition;
    public RobotBuilder robotBuilder;
    public int money = 40;
    public int removedHealth = 1;

    private void Start()
    {
        BattleBar.OnBarFull.AddListener(ShowMenu);
    }

    public void Win() => ShowMenu(EntityType.Player);
    public void Lose() => ShowMenu(EntityType.Enemy);

    public void ShowMenu(EntityType type)
    {
        SelectUI.SetActive(true);
        
        Backdrop.ColorTo(new Color(0, 0, 0, 0.93f), 0.8f, EaseType.OutCubic);

        var robot = robotBuilder.LoadRobot();
        robot.Head.PartHealth -= removedHealth;
        robot.Body.PartHealth -= removedHealth;
        robot.Weapon.PartHealth -= removedHealth;
        robot.MovementModule.PartHealth -= removedHealth;
        robot.Cpu.PartHealth -= removedHealth;
        robotBuilder.ChangeRobotPart(robot.Head);
        robotBuilder.ChangeRobotPart(robot.Body);
        robotBuilder.ChangeRobotPart(robot.Weapon);
        robotBuilder.ChangeRobotPart(robot.MovementModule);
        robotBuilder.ChangeRobotPart(robot.Cpu);

        if (type == EntityType.Player)
        {
            RankSystem.Win();
            MoneyBag.Add(money);
            WinScreen.MoveTo(new Vector3(7.3f, 0, -4), 1f, EaseType.OutCubic);
            return;
        }
        RankSystem.Loss();
        LoseScreen.MoveTo(new Vector3(20, 0, -4), 1f, EaseType.OutCubic);
    }

    public void LoadSceneTransitioned(int index)
    {
        Transition.In(0.01f, () =>
        {
            SceneManager.LoadScene(index);
        });
    }   
}
