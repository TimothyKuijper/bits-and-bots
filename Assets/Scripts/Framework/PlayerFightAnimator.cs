using System.Collections.Generic;
using UnityEngine;
using Yakanashe.Wiper;

public class PlayerFightAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string[] animationKeys;
    [SerializeField] private IconSwitcher iconSwitcher;
    [SerializeField] private Transition transition;

    private readonly List<string> remainingAnimations = new();

    private void Start()
    {
        iconSwitcher.onMatchMade.AddListener(PlayAttackAnimation);

        RefillAnimationBag();

        transition.Out(0.01f);
    }

    public void PlayAttackAnimation(int amount)
    {
        foreach (var key in animationKeys)
        {
            animator.ResetTrigger(key);
        }

        if (remainingAnimations.Count == 0) RefillAnimationBag();

        var index = Random.Range(0, remainingAnimations.Count);
        var animationName = remainingAnimations[index];
        remainingAnimations.RemoveAt(index);

        animator.CrossFade(animationName, 0.05f);
    }

    private void RefillAnimationBag()
    {
        remainingAnimations.Clear();
        remainingAnimations.AddRange(animationKeys);
    }
}