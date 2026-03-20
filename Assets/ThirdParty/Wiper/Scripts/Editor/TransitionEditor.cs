using UnityEditor;
using UnityEngine;
using Yakanashe.Wiper;

[CustomEditor(typeof(Transition))]
public class TransitionEditor : UnityEditor.Editor
{
    private const float CircleRadius = 50f;
    private const float BorderThickness = 1f;
    private const float RoundAngleTo = 5;

    private readonly Color _darkColor = new(0.22f, 0.22f, 0.22f);
    private readonly Color _lightColor = new(0.76f, 0.76f, 0.76f);

    private bool hasDirectionProperty;

    public override void OnInspectorGUI()
    {
        var wiper = (Transition)target;

        DrawDefaultInspector();

        GUILayout.Space(10);

        if (wiper.transitionCollection && wiper.transitionCollection.transitions.Count > 0)
        {
            var options = new string[wiper.transitionCollection.transitions.Count];
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = wiper.transitionCollection.transitions[i].name;
            }

            wiper.SelectedTransitionIndex = EditorGUILayout.Popup("Selected Transition", wiper.SelectedTransitionIndex, options);

            GUILayout.Space(10);
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a TransitionCollection to use transitions and edit their settings", MessageType.Warning);
        }

        var currentShader = wiper.transitionCollection.transitions[wiper.SelectedTransitionIndex].shader; // TODO: error if collection is unassigned while picking shader with index
        hasDirectionProperty = ShaderHasProperty(currentShader, "_Direction");

        if (hasDirectionProperty)
        {
            GUILayout.Label("Wipe Angle", EditorStyles.boldLabel);
            var controlRect = GUILayoutUtility.GetRect(120, 120, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            DrawAngleControl(controlRect, ref wiper.Angle);
            wiper.Angle = EditorGUILayout.FloatField(wiper.Angle);
        }

        if (!GUI.changed) return;

        EditorUtility.SetDirty(target);
        
        if (wiper.transitionCollection) return;
        hasDirectionProperty = false;
    }

    private void DrawAngleControl(Rect rect, ref float angle)
    {
        var center = rect.center;

        // circle border
        Handles.color = EditorGUIUtility.isProSkin ? _lightColor : _darkColor;
        Handles.DrawSolidDisc(center, Vector3.forward, CircleRadius + BorderThickness);

        // circle inner
        Handles.color = EditorGUIUtility.isProSkin ? _darkColor : _lightColor;
        Handles.DrawSolidDisc(center, Vector3.forward, CircleRadius);

        // https://discussions.unity.com/t/vector2-from-an-angle/863372/4
        var radians = -angle * Mathf.Deg2Rad; // direction is calculated using radians
        var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        var angleTip = center + direction * CircleRadius;

        var centerHandlePos = new Vector3(center.x, center.y, -10);
        var angleTipHandlePos = new Vector3(angleTip.x, angleTip.y, -10);

        Handles.color = _lightColor;
        Handles.DrawLine(centerHandlePos, angleTipHandlePos);

        var evt = Event.current;
        if (evt.type != EventType.MouseDrag || evt.button != 0) return;

        var mousePos = evt.mousePosition;
        var diff = center - mousePos;

        angle = Mathf.Atan2(diff.y, -diff.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        angle = RoundAngleTo * (int)Mathf.Round(angle / RoundAngleTo);

        GUI.changed = true;
    }

    private void OnValidate()
    {
        GUI.changed = true;
    }

    // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/ShaderUtil.html
    private static bool ShaderHasProperty(Shader shader, string propertyName)
    {
        if (shader == null) return false;

        int propertyCount = ShaderUtil.GetPropertyCount(shader);
        for (int i = 0; i < propertyCount; i++)
        {
            if (ShaderUtil.GetPropertyName(shader, i) == propertyName)
            {
                return true;
            }
        }

        return false;
    }
}