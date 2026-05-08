using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yakanashe.Wiper;
using Yakanashe.Yautl;
using EaseType = Yakanashe.Yautl.EaseType;

public class WinLoseArena : MonoBehaviour
{
    public Transform WinScreen, LoseScreen;
    public GameObject SelectUI;
    public BattleBar BattleBar;
    public Transition Transition;

    private void Start()
    {
        BattleBar.OnBarFull.AddListener(ShowMenu);
    }

    public void ShowMenu(EntityType type)
    {
        SelectUI.SetActive(true);
        if (type == EntityType.Player)
        {
            RankSystem.Win();
            WinScreen.MoveTo(new Vector3(7, 0, 0), 1f, EaseType.OutCubic);
            return;
        }
        RankSystem.Loss();
        LoseScreen.MoveTo(new Vector3(20, 0, 0), 1f, EaseType.OutCubic);
    }

    public void LoadSceneTransitioned(int index)
    {
        Transition.In(0.01f, () =>
        {
            SceneManager.LoadScene(index);
        });
    }   
}
