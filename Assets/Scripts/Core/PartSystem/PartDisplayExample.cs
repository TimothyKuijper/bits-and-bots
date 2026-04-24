using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yakapedia;

public class PartDisplayExample : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
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
    [SerializeField] private TileBar partHealth;
    [SerializeField] private TileBar partHealthBack;

    [Header("Dependencies")]
    [SerializeField] private PartCollection partCollection;

    private Part _lastPart;
    private const string PartKey = "SavedPart";


    private void Start()
    {
        saveButton.onClick.AddListener(() => PersistentData.Set(PartKey, _lastPart));
        loadButton.onClick.AddListener(() => DisplayPart(PersistentData.Get(PartKey, partCollection.parts[0])));
        rerollButton.onClick.AddListener(Generate);
        scalingSlider.onValueChanged.AddListener((value) => scalingLabel.text = "Scale: " + value.RoundToNearest(0.01f).ToString());
    }

    private void Generate()
    {
        var part = typeDropdown.value > 0 ? partCollection.GetRandomPartByType((Part.PartType)(typeDropdown.value - 1)) : partCollection.GetRandomPart();
        var scaledPart = PartUtility.ScalePart(part, scalingSlider.value);
        DisplayPart(scaledPart);
    }

    private void DisplayPart(Part part)
    {
        var intType = (int)part.Type;
        ColorUtility.TryParseHtmlString("#" + part.PartColor, out var newColor);

        partLabel.text = part.PartName;
        typeLabel.text = typeStrings[intType];
        rarityLabel.text = "Tier: " + part.Rarity.ToString();
        partImage.sprite = partTextures[intType];
        partImage.color = newColor;
        valueLabel.text = "Damage: " + part.GetPartValue().ToString();
        modifierLabel.text = "Modifier: " + part.BreakModifier.ToString();
        partHealth.Value = part.PartHealth;
        partHealthBack.Value = part.MaxPartHealth;

        _lastPart = part;
    }
}
