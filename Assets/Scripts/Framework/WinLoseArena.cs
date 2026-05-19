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

    private void Start()
    {
        BattleBar.OnBarFull.AddListener(ShowMenu);
    }

    public void ShowMenu(EntityType type)
    {
        SelectUI.SetActive(true);
        
        Backdrop.ColorTo(new Color(0, 0, 0, 0.93f), 0.8f, EaseType.OutCubic);
        
        if (type == EntityType.Player)
        {
            RankSystem.Win();
            MoneyBag.Add(10);
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
