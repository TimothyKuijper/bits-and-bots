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

    private void Start()
    {
        battleBarMaterial.ShaderFloatTo("_Fill", _barFillAmount * 0.01f, 0f, EaseType.Linear);
    }

    public void Add(EntityType type, float amount)
    {
        OnBarChanged.Invoke(type, amount);
        
        _barFillAmount += type == EntityType.Player ? amount : -amount;
        
        _sliderTween?.Stop();
        _sliderTween = battleBarMaterial.ShaderFloatTo("_Fill", _barFillAmount * 0.01f, 0.4f, EaseType.OutCubic);

        if (_barFillAmount is > 0 and < 100) return;
        OnBarFull.Invoke(_barFillAmount >= 100 ? EntityType.Player : EntityType.Enemy);
        _barFillAmount = Mathf.Clamp(_barFillAmount, 0, 100);
    }

    public void ResetBar()
    {
        _barFillAmount = 50;
        _sliderTween?.Stop();
        _sliderTween = battleBarMaterial.ShaderFloatTo("_Fill", _barFillAmount * 0.01f, 0.4f, EaseType.OutCubic);
    }

    private void OnDisable()
    {
        battleBarMaterial.SetFloat("_Fill", 0.5f);
    }
}

public enum EntityType
{
    Player,
    Enemy
}
