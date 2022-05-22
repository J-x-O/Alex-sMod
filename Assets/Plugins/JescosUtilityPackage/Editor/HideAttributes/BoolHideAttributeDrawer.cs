using UnityEditor;
using Utility.Attributes;

namespace JescoDev.Utility.HideAttributes.Editor.Plugins.Editor.HideAttributes {
    [CustomPropertyDrawer(typeof(BoolHideAttribute))]
    public class BoolHideAttributeDrawer : HideAttributeDrawer<BoolHideAttribute> {
        protected override bool SolveAttributeValue(BoolHideAttribute castedAttribute, SerializedProperty target) {
            return target.boolValue;
        }
    }
}