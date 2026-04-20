using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PartRollButton : MonoBehaviour
{
    [SerializeField] private PartCollection partCollection;
    // ADD SCALING OBJECT HERE
    // MONEY FOR COST OBJECT HERE
    [SerializeField] private PartsScreen partScreen;

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            partScreen.ComparePart(partCollection.GetRandomPart());
        });
    }
}
