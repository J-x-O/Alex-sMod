using System;
using UnityEngine;

namespace Utility.Attributes {

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
    public abstract class HideAttribute : PropertyAttribute {
        
        /// <summary> The name of the field that will be in control </summary>
        public string ConditionalSourceField;
    
        // <summary> TRUE = Hide in inspector / FALSE = Disable in inspector </summary>
        public bool HideInInspector;
        
        // <summary> TRUE = Hide in inspector / FALSE = Disable in inspector </summary>
        public bool Invert;

        protected HideAttribute(string conditionalSourceField) {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = false;
        }

        protected HideAttribute(string conditionalSourceField, bool hideInInspector) {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
        }
        
        protected HideAttribute(string conditionalSourceField, bool hideInInspector, bool invert) {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
            Invert = invert;
        }
        
    }

}