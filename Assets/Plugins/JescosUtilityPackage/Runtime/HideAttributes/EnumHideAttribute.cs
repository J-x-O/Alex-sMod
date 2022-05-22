using System;
using UnityEngine;

namespace Utility.Attributes {

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
    public class EnumHideAttribute : HideAttribute {

        /// <summary> The Value the enum field will be checked against </summary>
        public int[] ComparedEnumValue;

        public EnumHideAttribute(string conditionalSourceField, int[] comparedEnumValue)
            : base(conditionalSourceField) {
            
            ComparedEnumValue = comparedEnumValue;
        }

        public EnumHideAttribute(string conditionalSourceField, int[] comparedEnumValue, bool hideInInspector) 
            : base(conditionalSourceField, hideInInspector) {
            
            ComparedEnumValue = comparedEnumValue;
        }
        
        public EnumHideAttribute(string conditionalSourceField, int[] comparedEnumValue, bool hideInInspector, bool invert) 
            : base(conditionalSourceField, hideInInspector, invert) {
            
            ComparedEnumValue = comparedEnumValue;
        }
    }

}