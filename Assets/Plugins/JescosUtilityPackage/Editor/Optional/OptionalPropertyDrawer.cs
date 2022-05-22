#if (UNITY_EDITOR)

using System;
using UnityEditor;
using UnityEngine;

namespace Beta.Devtools {

    /// <summary> Custom Property Drawer for the optional struct, makes it look pretty uwu </summary>
    [CustomPropertyDrawer(typeof(Optional<>))]
    public class OptionalPropertyDrawer : PropertyDrawer {
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty valueProperty = property.FindPropertyRelative("Value");
            return EditorGUI.GetPropertyHeight(valueProperty);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            DrawOptionalBase(position, property, label, DrawValueProperty);
        }

        private void DrawValueProperty(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PropertyField(position, property, label, true);
        }

        public static void DrawOptionalBase(Rect position, SerializedProperty property, GUIContent label,
            Action<Rect, SerializedProperty, GUIContent> contentCallback) {
            
            SerializedProperty valueProperty = property.FindPropertyRelative("Value");
            SerializedProperty enabledProperty = property.FindPropertyRelative("Enabled");
            
            const float enSize = 18;
            float indentW = EditorGUI.IndentedRect(position).width;
            Rect valPosition = new Rect(position.x, position.y, position.width - enSize, position.height);
            Rect enPosition = new Rect(position.x + indentW - enSize + 4, position.y, enSize, enSize);
            
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
            contentCallback.Invoke(valPosition, valueProperty, label);
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.PropertyField(enPosition, enabledProperty, GUIContent.none);
            
            EditorGUI.EndProperty();
        }
    }
}

#endif