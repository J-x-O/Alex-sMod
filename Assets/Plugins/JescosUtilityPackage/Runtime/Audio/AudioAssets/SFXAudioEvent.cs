using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using GD.MinMaxSlider;
using UnityEngine.Serialization;

namespace Beta.Audio {

    /// <summary>Stores sound events</summary>
    /// <author>Written by Nikolas</author>
    [CreateAssetMenu(menuName = "ScriptableObjects/Audio/SFX")]
    public class SFXAudioEvent : AudioEvent {

        /// <summary> All AudioClips for this event, randomly choosen on play </summary>
        public IReadOnlyList<AudioClip> Clips => _clips;

        [FormerlySerializedAs("clips")]
        [Tooltip("All AudioClips for this event, randomly choosen on play")]
        [SerializeField] private List<AudioClip> _clips;

        /// <summary> The minimal and maximal volume, when one Clip is played </summary>
        public Vector2 Volume => _volume;
        
        [FormerlySerializedAs("volume")]
        [Tooltip("The minimal and maximal volume, when one Clip is played")]
        [MinMaxSlider(0, 2)]
        [SerializeField] private Vector2 _volume = new Vector2(0.9f, 1.1f);

        /// <summary> The minimal and maximal pitch, when one Clip is played </summary>
        public Vector2 Pitch => _pitch;
        
        [FormerlySerializedAs("pitch")]
        [Tooltip("The minimal and maximal pitch, when one Clip is played")]
        [MinMaxSlider(0, 2)]
        [SerializeField] private Vector2 _pitch = new Vector2(0.9f, 1.1f);

        /// <summary>Plays the event</summary>
        /// <param name="source">AudioSource, on which the event is played</param>
        /// <param name="setVolume">Does not apply here, is just needed for implementation of abstract method</param>
        public override void Play(AudioSource source, bool setVolume = true) {

            if (_clips.Count == 0) return;

            source.clip = _clips[Random.Range(0, _clips.Count)];
            source.volume = Random.Range(_volume.x, _volume.y);
            source.pitch = Random.Range(_pitch.x, _pitch.y);
            source.Play();
        }
    }
}