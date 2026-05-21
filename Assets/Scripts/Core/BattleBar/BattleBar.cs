using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yakanashe.Yautl;

public class BattleBar : MonoBehaviour
{
    public UnityEvent<EntityType> OnBarFull;
    public UnityEvent<EntityType, float> OnBarChanged;

    [SerializeField] private Material battleBarMaterial;
    
    private float _barFillAmount = 50;
    private ITween _sliderTween;
    
    private bool _finished;
    private bool _allowDamage;

    private void Start()
    {
        ResetBar();
        Invoke(nameof(EnableDamage), 0.3f);
    }

    public void Add(EntityType type, float amount)
    {
        if (_finished || !_allowDamage) return;
        
        Debug.Log($"{type} did {amount} damage");
        
        OnBarChanged.Invoke(type, amount);
        
        _barFillAmount += type == EntityType.Player ? amount : -amount;
        
        _sliderTween?.Stop();
        _sliderTween = battleBarMaterial.ShaderFloatTo("_Fill", _barFillAmount * 0.01f, 0.4f, EaseType.OutCubic);

        if(type == EntityType.Player) FlashBar(amount);

        if (_barFillAmount is > 0 and < 100) return;
        OnBarFull.Invoke(_barFillAmount >= 100 ? EntityType.Player : EntityType.Enemy);
        _finished = true;
        _barFillAmount = Mathf.Clamp(_barFillAmount, 0, 100);
    }

    public void ResetBar()
    {
        _finished = false;
        _barFillAmount = 50;
        battleBarMaterial.SetFloat("_Fill", _barFillAmount * 0.01f);
    }

    private void OnDisable()
    {
        battleBarMaterial.SetFloat("_Fill", 0.5f);
        battleBarMaterial.SetFloat("_FresnelStrength", -0.5f);
    }
    
    private void FlashBar(float amount)
    {
        battleBarMaterial.SetFloat("_FresnelStrength", -0.2f - amount * 0.2f);
        battleBarMaterial.ShaderFloatTo("_FresnelStrength", -0.5f, 0.3f, EaseType.OutCubic);
    }

    private void EnableDamage() => _allowDamage = true;
}

public enum EntityType
{
    Player,
    Enemy
}
