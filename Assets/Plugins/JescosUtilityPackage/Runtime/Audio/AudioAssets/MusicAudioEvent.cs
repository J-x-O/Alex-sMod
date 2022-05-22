using UnityEngine;
using System.Collections;

namespace Beta.Audio {

    /// <summary>Stores non-adaptive music events</summary>
    /// <author>Written by Nikolas</author>
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Simple Music")]
    public class MusicAudioEvent : AudioEvent {

        [Tooltip("Stores AudioClip")] [SerializeField]
        public AudioClip clip;


        [Tooltip("Stores Volume")] [Range(0f, 1f)] [SerializeField]
        public float volume;

        /// <summary>Plays the event</summary>
        /// <param name="source">AudioSource, on which the event is played</param>
        /// <param name="setVolume">If true, volume is set to the value specified in the inspector</param>
        public override void Play(AudioSource source, bool setVolume = true) {

            if (clip == null) return;

            source.clip = clip;

            if(setVolume) source.volume = volume;

            source.pitch = 1f;
            source.loop = true;
            source.Play();
            
        }
    }
}
