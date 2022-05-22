using UnityEngine;

namespace Beta.Audio {

    /// <summary> Parent class for audioevents </summary>
    public abstract class AudioEvent : ScriptableObject {

        /// <summary>Plays the event</summary>
        /// <param name="source">AudioSource, on which the event is played</param>
        /// <remarks>Nikolas</remarks>
        public abstract void Play(AudioSource source, bool setVolume = true);
        
    }
}

