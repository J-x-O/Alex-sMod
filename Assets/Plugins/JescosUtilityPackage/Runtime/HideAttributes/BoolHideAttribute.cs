using System;
using UnityEngine;

namespace Utility.Attributes {

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct,Inherited = true)]
    public class BoolHideAttribute : HideAttribute {

        /// <summary> The name of the bool field that will be in control </summary>
        public string ConditionalSourceField;
    
        // <summary> TRUE = Hide in inspector / FALSE = Disable in inspector </summary>
        public bool HideInInspector;

        public BoolHideAttribute(string conditionalSourceField) : base(conditionalSourceField) { }

        public BoolHideAttribute(string conditionalSourceField, bool hideInInspector)
            : base(conditionalSourceField, hideInInspector) { }
        
        public BoolHideAttribute(string conditionalSourceField, bool hideInInspector, bool invert)
            : base(conditionalSourceField, hideInInspector, invert) { }
    }

}