using System;
using UnityEngine;

namespace Beta.Devtools {

    /// <summary> A struct for storing Data that can be disabled </summary>
    /// <typeparam name="T"> The Type of Data that will be used </typeparam>
    [Serializable]
    public struct Optional<T> {
        
        /// <summary> If this Value is Enabled, and should be used </summary>
        public bool Enabled;
        
        /// <summary> The actual Value of the struct </summary>
        public T Value;

        /// <summary> The Base Constructor </summary>
        /// <param name="initialValue"> The initial Value of the struct </param>
        /// <param name="enabled"> If the Value is enabled at the beginning or not</param>
        public Optional(T initialValue, bool enabled = true) {
            Enabled = enabled;
            Value = initialValue;
        }
    }
}