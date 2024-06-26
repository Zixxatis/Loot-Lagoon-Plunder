using UnityEditor;
using UnityEngine;

namespace CGames.CustomEditors
{
    [CustomPropertyDrawer(typeof(ColoredGems))]
    public class ColoredGemsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            EditorGUI.indentLevel = 0;

            float fieldWidth = position.width / 3f;

            SerializedProperty red = property.FindPropertyRelative("Red");
            SerializedProperty green = property.FindPropertyRelative("Green");
            SerializedProperty blue = property.FindPropertyRelative("Blue");

            EditorGUIUtility.labelWidth = 12f;
            EditorGUI.PropertyField(new Rect(position.x, position.y, fieldWidth, position.height), red, new GUIContent("R"));
            EditorGUI.PropertyField(new Rect(position.x + fieldWidth, position.y, fieldWidth, position.height), green, new GUIContent(" G"));
            EditorGUI.PropertyField(new Rect(position.x + 2 * fieldWidth, position.y, fieldWidth, position.height), blue, new GUIContent(" B"));

            EditorGUI.EndProperty();
        }
    }
}