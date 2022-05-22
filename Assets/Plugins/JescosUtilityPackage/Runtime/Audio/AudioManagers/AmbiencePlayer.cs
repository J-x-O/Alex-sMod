using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Beta.Audio;
using Plugins.Audio.AudioManagers;
using UnityEngine;
using UnityEngine.Audio;

namespace Beta.Audio {

    /// <summary> Plays Ambience sounds</summary>
    public class AmbiencePlayer : GenericManager<AmbienceAudioEvent> {

        protected override AudioMixerGroup Group => SoundManager.Instance.EffectsAudioGroup;

        [Tooltip("How long until effects are faded in and out")]
        [SerializeField] private float _fadeDuration;
        public float FadeDuration => _fadeDuration;

        /// <summary> Plays the provided Event </summary>
        /// <param name="ambienceEvent"> Name of the event, which shall be played </param>
        /// <param name="loop"> Whether the sound should loop or not. </param>
        /// <returns> the token that can be used to cancel the ambience </returns>
        public SoundToken<AmbienceAudioEvent> PlayAmbience(AmbienceAudioEvent ambienceEvent, bool loop, float fadeDuration = 0) {
            return PlaySound(ambienceEvent, 0, fadeDuration == 0 ? _fadeDuration : fadeDuration, loop);
        }

        /// <summary> Fade out a certain played Ambience </summary>
        /// <param name="token"> the token received when playing this sound </param>
        public void StopAmbience(SoundToken<AmbienceAudioEvent> token) => StopSoundWithFadeOut(token);
    }

}
