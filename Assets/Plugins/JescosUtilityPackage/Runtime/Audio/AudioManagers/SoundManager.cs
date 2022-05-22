using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.Audio.AudioManagers {

    public class SoundManager : MonoBehaviour {
    
        /// <summary>Static instance of the GameSettingsManager.</summary>
        public static SoundManager Instance { get; private set; }

        public AudioMixerGroup MusicAudioGroup => _musicAudioGroup;
        [SerializeField] private AudioMixerGroup _musicAudioGroup;

        public AudioMixerGroup EffectsAudioGroup => _effectsAudioGroup;
        [SerializeField] private AudioMixerGroup _effectsAudioGroup;

        public AudioMixerGroup VoicesAudioGroup => _voicesAudioGroup;
        [SerializeField] private AudioMixerGroup _voicesAudioGroup;
    
        public AudioMixerGroup AmbienceAudioGroup => _ambienceAudioGroup;
        [SerializeField] private AudioMixerGroup _ambienceAudioGroup;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }
    
    }

}
