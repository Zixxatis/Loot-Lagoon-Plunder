using UnityEditor;
using UnityEngine;

namespace CGames.CustomEditors
{
    [CustomPropertyDrawer(typeof(GemsRewardConfig))]
    public class GemsRewardConfigDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            bool isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property.isExpanded, label, true);
            property.isExpanded = isExpanded;

            if (isExpanded)
            {
                SerializedProperty hasRandomRewardsProperty = property.FindPropertyRelative("HasRandomRewards");
                SerializedProperty rewardPoolProperty = property.FindPropertyRelative("RewardsPool");
                SerializedProperty rewardProperty = property.FindPropertyRelative("Reward");
                SerializedProperty delayProperty = property.FindPropertyRelative("delayBeforeInstantiatingReward");
                
                EditorGUILayout.PropertyField(hasRandomRewardsProperty);

                if (hasRandomRewardsProperty.boolValue)
                    EditorGUILayout.PropertyField(rewardPoolProperty, true);
                else
                    EditorGUILayout.PropertyField(rewardProperty);

                EditorGUILayout.PropertyField(delayProperty);
            }

            EditorGUI.EndProperty();
        }
    }
}