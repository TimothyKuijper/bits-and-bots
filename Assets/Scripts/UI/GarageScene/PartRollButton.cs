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
    [SerializeField] private PartsScreen partScreen;

    public UnityEvent OnEnough;
    public UnityEvent OnNotEnough;

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (MoneyBag.HasEnough(cost) == false)
            {
                OnNotEnough.Invoke();
                return;
            }
            MoneyBag.Remove(cost);
            OnEnough.Invoke();

            var part = PartUtility.ScalePart(partCollection.GetRandomPart(), RankSystem.CurrentRank);
            partScreen.ComparePart(part);
        });

        var buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Roll for part - $ " + cost.ToString();
    }
}
