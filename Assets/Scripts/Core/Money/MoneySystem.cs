using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public int _money = 0;
    public TextMeshProUGUI _moneyText;

    private const string CoinsKey = "PlayerMoney";
    
    public void AddMoney()
    {
        _money += 1;
        SaveMoney();
        UpdateUI();
    }
    
    void UpdateUI()
    {
        _moneyText.text = "Money: " + _money;
    }

    void SaveMoney()
    {
        PlayerPrefs.SetInt(CoinsKey, _money);
        PlayerPrefs.Save();
    }

    void LoadMoney()
    {
        _money = PlayerPrefs.GetInt(CoinsKey);
    }

    void Start()
    {
        LoadMoney();
        UpdateUI();
    }
}
