using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Plugins.Audio.AudioManagers;
using UnityEngine.Audio;

namespace Beta.Audio {
    
    /// <summary> Manages vfx-events </summary>
    public class SFXManager : GenericManager<SFXAudioEvent> {
        protected override AudioMixerGroup Group => SoundManager.Instance.EffectsAudioGroup;

        public void PlaySimpleSFX(SFXAudioEvent sfxEvent) => PlaySFX(sfxEvent);
        
        /// <summary> Plays the provided Event </summary>
        /// <param name="sfxEvent"> Name of the event, which shall be played </param>
        /// <param name="delay"> the duration between this method call and the start of the vfx </param>
        /// /// <param name="fadeDuration"> the time it takes until the effect is turned up and down </param>
        /// <param name="loop"> if the effect should loop, if true it can only be canceled with the token </param>
        /// <returns> the token that can be used to cancel the vfx </returns>
        public SoundToken<SFXAudioEvent> PlaySFX(SFXAudioEvent sfxEvent, float delay = 0, float fadeDuration = 0, bool loop = false) {
            return PlaySound(sfxEvent, delay, fadeDuration, loop);
        }

        /// <summary> Stops a certain played Sound </summary>
        /// <param name="token"> the token received when playing this sound </param>
        public void StopSFX(SoundToken<SFXAudioEvent> token) => StopSound(token);

        /// <summary> Keeps the GameObject of the SFXManager alive until all sounds on it finished playing. </summary>
        public void DestroySFXManagerAfterSoundEnded() {
            
            // de-parent so the object doesn't get destroyed when the projectile dies
            transform.parent = null;
            
            float longestTimeLeftToPlay = 0;
            
            // go through every currently registered sound of this manager...
            foreach (KeyValuePair<SoundToken<SFXAudioEvent>, Coroutine> runningSound in RunningVFX) {
                // ...check if the source still exists (could be gone due to pausing)...
                if (runningSound.Key.Source == null) continue;
                
                // ...check if the remaining duration of this clip is longer than the remaining duration of any other clip...
                float tempTime = runningSound.Key.Source.clip.length - runningSound.Key.Source.time;
                
                // ...if yes, store this duration...
                if (longestTimeLeftToPlay < tempTime) {
                    longestTimeLeftToPlay = tempTime;
                }
            }

            // ...and use it to determine after what time the SFXManager can be destroyed, since all sounds will have stopped playing by then
            Destroy(gameObject, longestTimeLeftToPlay);
        }
    }
}
