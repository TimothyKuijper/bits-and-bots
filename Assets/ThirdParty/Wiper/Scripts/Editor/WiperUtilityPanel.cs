using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WiperUtilityPanel : EditorWindow
{
    [SerializeField] private List<UniversalRendererData> _targetRenderers;
    [SerializeField] private SerializedProperty _renderersProperty;
    
    private const string DefaultMaterialPath = "Assets/Yakanashe/Wiper/Materials/WiperMaterial.mat";
    private const string DefaultBannerPath = "Assets/Yakanashe/Wiper/Textures/Banner.png";
    
    private Material _wiperMaterial;
    private Texture2D _banner;
    [SerializeField] private SerializedObject _serializedWindow;
    
    [MenuItem("Tools/Wiper/Utility Panel")]
    public static void ShowWindow()
    {
        var window = GetWindow<WiperUtilityPanel>("Wiper Utility Panel");
        
        window._serializedWindow = new SerializedObject(window);
        window._renderersProperty = window._serializedWindow.FindProperty("_targetRenderers");
        
        window._targetRenderers = GetAllUniversalRendererData();
        
        window._banner = AssetDatabase.LoadAssetAtPath<Texture2D>(DefaultBannerPath);
        window._wiperMaterial = AssetDatabase.LoadAssetAtPath<Material>(DefaultMaterialPath);
        
    }

    private void OnGUI()
    {
        if (_banner != null)
        {
            GUILayout.Label(_banner, GUILayout.Width(position.width - 10), GUILayout.Height(position.width / 3));
        }
        
        EditorGUI.HelpBox(GUILayoutUtility.GetRect(position.width, 50), "Wiper automatically attempts to fill in the Material and Renderer Data fields. If these fields are empty, you'll need to provide the necessary data manually to create a render feature", MessageType.Info);
        
        EditorGUILayout.Space();

        _wiperMaterial = (Material)EditorGUILayout.ObjectField("Wiper Material", _wiperMaterial, typeof(Material), false);
        if (!_wiperMaterial)
        {
            EditorGUI.HelpBox(GUILayoutUtility.GetRect(position.width, 40), "Please select a Material to use in the creation process of the Render Feature.", MessageType.Warning);
            return;
        }
        
        _serializedWindow.Update();
        EditorGUILayout.PropertyField(_renderersProperty, true);
        _serializedWindow.ApplyModifiedProperties();

        EditorGUILayout.Space();
        
        if (GUILayout.Button("Apply Render Feature"))
        {
            EnsureFullscreenPass(_targetRenderers, _wiperMaterial);
        }
    }

    private static void EnsureFullscreenPass(List<UniversalRendererData> targetRenderers, Material material)
    {
        // https://github.com/JetBrains/resharper-unity/wiki/Possible-unintended-bypass-of-lifetime-check-of-underlying-Unity-engine-object
        // should be good and return if targetRenderers is null but in not sure lol
        if (targetRenderers?.Count == 0 || !material)
        {
            Debug.Log("Target Renderer list empty or there is no material");
            return;
        }

        foreach (var targetRenderer in targetRenderers)
        {
            if (!targetRenderer)
            {
                continue;
            }

            var featureExists = false;
            
            foreach (var feature in targetRenderer.rendererFeatures)
            {
                // using 'is not' so i can make it a variable, otherwise it'll error because not every renderer feature has a passMaterial
                if (feature is not FullScreenPassRendererFeature fullscreenFeature || fullscreenFeature.passMaterial != material) continue;
                featureExists = true;
                break;
            }

            if (featureExists)
            {
                Debug.Log($"Pass already exists in {targetRenderer.name}, skipping");
                continue;
            }

            // https://discussions.unity.com/t/urp-adding-a-renderfeature-from-script/842637/4
            var newFeature = CreateInstance<FullScreenPassRendererFeature>();
            newFeature.name = "FullscreenPass_" + material.name;
            newFeature.passMaterial = material;

            targetRenderer.rendererFeatures.Add(newFeature);
            targetRenderer.SetDirty();

            Debug.Log($"Added pass with {material.name} to {targetRenderer.name}");
        }
        
        if (EditorUtility.DisplayDialog("Wiper", "Fullscreen render feature applied!", "Thank you!", "Ok"))
        {
            // i love this little easter egg
            EditorUtility.DisplayDialog("Wiper", "No problem! Good luck with Wiper!", "Ok");
        }
    }
    
    public static List<UniversalRendererData> GetAllUniversalRendererData()
    {
        var rendererDataList = new List<UniversalRendererData>();
        
        // https://stackoverflow.com/questions/29526625/how-to-find-all-assets-of-a-type
        string[] guids = AssetDatabase.FindAssets("t:UniversalRendererData");
        foreach (string guid in guids) // speciaal voor rider: fuck linq, ik wil het NIET gebruiken
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            
            // exclude the default one from unity, yucky :(
            if (path.StartsWith("Packages/")) continue;
            
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(path);
            if (rendererData == null) continue;
            
            rendererDataList.Add(rendererData);
        }

        return rendererDataList;
    }
}