using UnityEngine;

namespace Beta.Devtools.Attributes {

    public class OptionalRange  : PropertyAttribute  {
 
        public float min;
        public float max;
 
        public OptionalRange(float min, float max) {
            this.min = min;
            this.max = max;
        }
    }

}