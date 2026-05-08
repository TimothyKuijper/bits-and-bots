using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class RankBar : MonoBehaviour
{
    private Slider _slider;  
       
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.minValue = 0;
        _slider.maxValue = RankSystem.XpThreshold;
        UpdateBar();
    }

    public void UpdateBar()
    {
        _slider.value = RankSystem.CurrentXp;
    }
}
