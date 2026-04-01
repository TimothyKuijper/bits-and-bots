using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yakapedia;

public class PartDisplayExample : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private Button rerollButton;
    [SerializeField] private Slider scalingSlider;
    [SerializeField] private TextMeshProUGUI scalingLabel;
    [SerializeField] private TMP_Dropdown typeDropdown;

    [Header("Generation Display")]
    [SerializeField] private TextMeshProUGUI partLabel;
    [SerializeField] private TextMeshProUGUI typeLabel;
    [SerializeField] private TextMeshProUGUI rarityLabel;
    [SerializeField] private List<string> typeStrings = new List<string>();
    [SerializeField] private Image partImage;
    [SerializeField] private List<Sprite> partTextures = new List<Sprite>();
    [SerializeField] private TextMeshProUGUI valueLabel;
    [SerializeField] private TextMeshProUGUI modifierLabel;
    //[SerializeField] private TextureBar partHealth;

    [Header("Dependencies")]
    [SerializeField] private PartCollection partCollection;

    private void Start()
    {
        rerollButton.onClick.AddListener(Generate);
        scalingSlider.onValueChanged.AddListener((value) => scalingLabel.text = "Scale: " + value.RoundToNearest(0.01f).ToString());
    }

    private void Generate()
    {
        var part = typeDropdown.value > 0 ? partCollection.GetRandomPartByType((Part.PartType)(typeDropdown.value - 1)) : partCollection.GetRandomPart();
        var scaledPart = PartUtility.ScalePart(part, scalingSlider.value);
        var intType = (int)scaledPart.Type;

        partLabel.text = scaledPart.PartName;
        typeLabel.text = typeStrings[intType];
        rarityLabel.text = "Tier: " + scaledPart.Rarity.ToString();
        partImage.sprite = partTextures[intType];
        partImage.color = scaledPart.PartColor;
        valueLabel.text = "Damage: " + scaledPart.GetPartValue().ToString();
        modifierLabel.text = "Modifier: " + scaledPart.BreakModifier.ToString();
        //Health
    }
}
