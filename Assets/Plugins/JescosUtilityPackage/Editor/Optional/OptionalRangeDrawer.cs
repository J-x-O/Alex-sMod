using UnityEngine;
using UnityEditor;

namespace Beta.Devtools.Attributes.Editor {

    [CustomPropertyDrawer(typeof(OptionalRange))]
    public class OptionalRangeDrawer : PropertyDrawer {
     
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            OptionalPropertyDrawer.DrawOptionalBase(position, property, label, DrawRange);
        }

        private void DrawRange(Rect position, SerializedProperty property, GUIContent label) {
            
            // First get the attribute since it contains the range for the slider
            OptionalRange range = attribute as OptionalRange;
            
            // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
            switch (property.propertyType) {
                
                case SerializedPropertyType.Float:
                    EditorGUI.Slider(position, property, range.min, range.max, label);
                    break;
                
                case SerializedPropertyType.Integer:
                    EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, label);
                    break;
                
                default:
                    EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
                    break;
            }
        }
    }
}
    