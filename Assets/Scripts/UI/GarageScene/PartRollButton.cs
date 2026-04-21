using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PartRollButton : MonoBehaviour
{
    [SerializeField] private int cost = 10;
    [SerializeField] private PartCollection partCollection;
    // ADD SCALING OBJECT HERE
    [SerializeField] private PartsScreen partScreen;

    public UnityEvent OnEnough;
    public UnityEvent OnNotEnough;

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (MoneyBag.HasEnoughMoney(cost) == false)
            {
                OnNotEnough.Invoke();
                return;
            }
            MoneyBag.RemoveMoney(cost);

            OnEnough.Invoke();
            partScreen.ComparePart(partCollection.GetRandomPart());
        });

        var buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Roll for part - $ " + cost.ToString();
    }
}
