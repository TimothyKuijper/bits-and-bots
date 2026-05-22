using System.Collections.Generic;
using UnityEngine;
using Yakanashe.Wiper;

public class EnemyFightAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string[] animationKeys;
    [SerializeField] private TickRunner tickRunner;

    private void Start()
    {
        tickRunner.OnAttack.AddListener(PlayAttackAnimation);
    }

    public void PlayAttackAnimation(float amount)
    {
        var index = Mathf.RoundToInt(Random.value);
        animator.CrossFade(animationKeys[index], 0.05f);
    }
}