using UnityEngine;
using UnityEditor;

namespace CGames.CustomEditors
{
    [CustomPropertyDrawer(typeof(StateClip))]
    public class StateClipDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            bool isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property.isExpanded, label, true);
            property.isExpanded = isExpanded;

            if (isExpanded)
            {
                GUIStyle boldStyle = new(GUI.skin.label)
                {
                    fontStyle = FontStyle.Bold
                };

                SerializedProperty hasMultipleAnimationsProperty = property.FindPropertyRelative("hasMultipleAnimations");
                SerializedProperty animationClipProperty = property.FindPropertyRelative("animationClip");
                SerializedProperty animationClipsListProperty = property.FindPropertyRelative("animationClipsList");

                EditorGUILayout.LabelField("Animation(s)", boldStyle);
                EditorGUILayout.PropertyField(hasMultipleAnimationsProperty);

                if (hasMultipleAnimationsProperty.boolValue)
                    EditorGUILayout.PropertyField(animationClipsListProperty, true);
                else
                    EditorGUILayout.PropertyField(animationClipProperty);


                SerializedProperty soundEffectClipProperty = property.FindPropertyRelative("soundEffectClip");
                SerializedProperty shouldPlayAsOneShotProperty = property.FindPropertyRelative("shouldPlayAsOneShot");
                SerializedProperty shouldPlayOnlyWhenVisibleProperty = property.FindPropertyRelative("shouldPlayOnlyWhenVisible");
                SerializedProperty shouldSetOnLoopProperty = property.FindPropertyRelative("shouldSetOnLoop");

                EditorGUILayout.LabelField("Sound Effect", boldStyle);
                EditorGUILayout.PropertyField(soundEffectClipProperty);
                EditorGUILayout.PropertyField(shouldPlayAsOneShotProperty);

                if (shouldPlayAsOneShotProperty.boolValue)
                    EditorGUILayout.PropertyField(shouldPlayOnlyWhenVisibleProperty);
                else
                    EditorGUILayout.PropertyField(shouldSetOnLoopProperty);
            }

            EditorGUI.EndProperty();
        }
    }
}