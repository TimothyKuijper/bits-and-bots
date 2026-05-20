using System;
using UnityEngine;
using Yakanashe.Wiper;

public class PlayerFightAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string jabKey;
    [SerializeField] private IconSwitcher iconSwitcher;

    [SerializeField] private Transition transition;

    private void Start()
    {
        iconSwitcher.onMatchMade.AddListener(PlayJabAnimation);
        transition.Out(0.01f);
    }

    public void PlayJabAnimation(int amount)
    {
        animator.SetTrigger(jabKey);
        Invoke(nameof(ResetJabKet), 1f);
    }

    private void ResetJabKet()
    {
        animator.ResetTrigger(jabKey);
    }
    
    
}
