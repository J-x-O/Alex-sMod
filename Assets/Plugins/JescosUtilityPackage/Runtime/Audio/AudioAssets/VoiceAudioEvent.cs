using System.Collections.Generic;
using GD.MinMaxSlider;
using UnityEngine;
using UnityEngine.Serialization;

namespace Beta.Audio {

    /// <summary>Stores sound events</summary>
    /// <author>Written by Nikolas</author>
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Voice")]
    public class VoiceAudioEvent : AudioEvent {
        
        /// <summary> All AudioClips for this event, randomly choosen on play </summary>
        public AudioClip Clip => _clip;

        [FormerlySerializedAs("clips")]
        [Tooltip("All AudioClips for this event, randomly choosen on play")]
        [SerializeField] private AudioClip _clip;

        /// <summary> The minimal and maximal volume, when one Clip is played </summary>
        public float Volume => _volume;
        
        [FormerlySerializedAs("volume")]
        [Tooltip("The minimal and maximal volume, when one Clip is played")]
        [Range(0, 2)]
        [SerializeField] private float _volume = 1f;

        /// <summary>Plays the event</summary>
        /// <param name="source">AudioSource, on which the event is played</param>
        /// <param name="setVolume">Does not apply here, is just needed for implementation of abstract method</param>
        public override void Play(AudioSource source, bool setVolume = true) {

            source.clip = _clip;
            source.volume = _volume;
            source.pitch = 1;
            source.Play();
        }
    }

}