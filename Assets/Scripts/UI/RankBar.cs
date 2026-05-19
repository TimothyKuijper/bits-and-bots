using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class RankBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI xpText;

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
        rankText.text = $"Rank {RankSystem.CurrentRank}";
        xpText.text = $"{RankSystem.CurrentXp}/{RankSystem.XpThreshold}";
    }
}