using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public struct SerializableMatrix4x4
{
    public float m00, m01, m02, m03;
    public float m10, m11, m12, m13;
    public float m20, m21, m22, m23;
    public float m30, m31, m32, m33;

    public SerializableMatrix4x4(float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;
        this.m03 = m03;
        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;
        this.m13 = m13;
        this.m20 = m20;
        this.m21 = m21;
        this.m22 = m22;
        this.m23 = m23;
        this.m30 = m30;
        this.m31 = m31;
        this.m32 = m32;
        this.m33 = m33;
    }

    public static implicit operator Matrix4x4(SerializableMatrix4x4 sm)
    {
        return new Matrix4x4
        {
            m00 = sm.m00, m01 = sm.m01, m02 = sm.m02, m03 = sm.m03,
            m10 = sm.m10, m11 = sm.m11, m12 = sm.m12, m13 = sm.m13,
            m20 = sm.m20, m21 = sm.m21, m22 = sm.m22, m23 = sm.m23,
            m30 = sm.m30, m31 = sm.m31, m32 = sm.m32, m33 = sm.m33
        };
    }

    public static implicit operator SerializableMatrix4x4(Matrix4x4 m)
    {
        return new SerializableMatrix4x4
        (
            m.m00, m.m01, m.m02, m.m03,
            m.m10, m.m11, m.m12, m.m13,
            m.m20, m.m21, m.m22, m.m23,
            m.m30, m.m31, m.m32, m.m33
        );
    }

    public void TRS(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this = Matrix4x4.TRS(position, rotation, scale);
    }

    public static SerializableMatrix4x4 Identity =>
        new(1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);
    
    public static SerializableMatrix4x4 Zero =>
        new(0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0);

    public override string ToString()
    {
        return $"[{m00}, {m01}, {m02}, {m03}]\n[{m10}, {m11}, {m12}, {m13}]\n[{m20}, {m21}, {m22}, {m23}]\n[{m30}, {m31}, {m32}, {m33}]";
    }
}

[CustomPropertyDrawer(typeof(SerializableMatrix4x4))]
public class SerializableMatrix4x4Drawer : PropertyDrawer
{
    private const float CellHeight = 18f;
    private const float Padding = 5f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.PrefixLabel(position, label);

        var cellWidth = (position.width - Padding * 3) / 4;

        var m00 = property.FindPropertyRelative("m00");
        var m01 = property.FindPropertyRelative("m01");
        var m02 = property.FindPropertyRelative("m02");
        var m03 = property.FindPropertyRelative("m03");
        var m10 = property.FindPropertyRelative("m10");
        var m11 = property.FindPropertyRelative("m11");
        var m12 = property.FindPropertyRelative("m12");
        var m13 = property.FindPropertyRelative("m13");
        var m20 = property.FindPropertyRelative("m20");
        var m21 = property.FindPropertyRelative("m21");
        var m22 = property.FindPropertyRelative("m22");
        var m23 = property.FindPropertyRelative("m23");
        var m30 = property.FindPropertyRelative("m30");
        var m31 = property.FindPropertyRelative("m31");
        var m32 = property.FindPropertyRelative("m32");
        var m33 = property.FindPropertyRelative("m33");

        var cellRect = new Rect(position.x, position.y, cellWidth, CellHeight);
        DrawRow(ref cellRect, cellWidth, m00, m01, m02, m03);
        cellRect.y += CellHeight + Padding;
        DrawRow(ref cellRect, cellWidth, m10, m11, m12, m13);
        cellRect.y += CellHeight + Padding;
        DrawRow(ref cellRect, cellWidth, m20, m21, m22, m23);
        cellRect.y += CellHeight + Padding;
        DrawRow(ref cellRect, cellWidth, m30, m31, m32, m33);
    }

    private static void DrawRow(ref Rect cellRect, float cellWidth, params SerializedProperty[] properties)
    {
        foreach (var property in properties)
        {
            property.floatValue = EditorGUI.FloatField(cellRect, property.floatValue);
            cellRect.x += cellWidth + Padding;
        }

        cellRect.x -= (cellWidth + Padding) * properties.Length;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (CellHeight + Padding) * 4 - Padding;
    }
}