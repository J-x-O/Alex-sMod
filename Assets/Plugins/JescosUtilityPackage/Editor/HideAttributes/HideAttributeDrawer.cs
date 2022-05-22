using UnityEditor;
using UnityEngine;
using Utility.Attributes;

namespace JescoDev.Utility.HideAttributes.Editor.Plugins.Editor.HideAttributes {

    public abstract class HideAttributeDrawer<T> : PropertyDrawer where T : HideAttribute {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            T castedAttribute = (T) attribute;
            bool enabled = SolveTargetAttribute(castedAttribute, property);

            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;
            if (!castedAttribute.HideInInspector || enabled) EditorGUI.PropertyField(position, property, label, true);

            GUI.enabled = wasEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            T castedAttribute = (T) attribute;
            bool enabled = SolveTargetAttribute(castedAttribute, property);

            if (!castedAttribute.HideInInspector || enabled) return EditorGUI.GetPropertyHeight(property, label);

            return -EditorGUIUtility.standardVerticalSpacing;
        }


        private bool SolveTargetAttribute(T castedAttribute, SerializedProperty property) {
            //returns the property path of the property we want to apply the attribute to
            string propertyPath = property.propertyPath;

            //changes the path to the conditional source property path
            string conditionPath = propertyPath.Replace(property.name, castedAttribute.ConditionalSourceField);
            SerializedProperty target = property.serializedObject.FindProperty(conditionPath);

            if (target != null) {
                bool value = SolveAttributeValue(castedAttribute, target);
                return castedAttribute.Invert ? !value : value;
            }

            Debug.LogWarning(
                "Attempting to use a EnumHideAttribute but no matching SourcePropertyValue found in object: " +
                castedAttribute.ConditionalSourceField);
            return true;
        }

        protected abstract bool SolveAttributeValue(T castedAttribute, SerializedProperty property);
    }
}