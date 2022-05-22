using System;
using System.Collections;
using System.Collections.Generic;
using Beta.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.Audio.AudioManagers {

    /// <summary> A Token created when an effect is started, identifying the playing sound </summary>
    public class SoundToken<T> where T : AudioEvent {
        public T Event { get; }
        public AudioSource Source { get; }
        public float Delay { get; }
        public float FadeDuration { get; }
        public bool Loop { get; }
        
        public SoundToken(T @event, AudioSource source, float delay, float fadeDuration, bool loop) {
            Event = @event;
            Source = source;
            Delay = delay;
            FadeDuration = fadeDuration;
            Loop = loop;
        }
    }
    
    /// <summary> Generic Class managing any AudioEvent </summary>
    /// <author> Written by Nikolas </author>
    public abstract class GenericManager<T> : MonoBehaviour where T : AudioEvent {
        
        protected abstract AudioMixerGroup Group { get; }
        
        private Dictionary<SoundToken<T>, Coroutine> _runningVFX = new Dictionary<SoundToken<T>, Coroutine>();

        public Dictionary<SoundToken<T>, Coroutine> RunningVFX => _runningVFX;

        /// <summary>Plays the event</summary>
        /// <param name="vfxEvent"> Name of the event, which shall be played </param>
        /// <param name="delay"> the duration between this method call and the start of the vfx </param>
        /// /// <param name="fadeDuration"> the time it takes until the effect is turned up and down </param>
        /// <param name="loop"> if the effect should loop, if true it can only be canceled with the token </param>
        /// <returns> the token that can be used to cancel the vfx </returns>
        protected SoundToken<T> PlaySound(T vfxEvent, float delay = 0, float fadeDuration = 0, bool loop = false) {

            SoundToken<T> token = new SoundToken<T>(vfxEvent, gameObject.AddComponent<AudioSource>(),
                delay, fadeDuration, loop);
            Coroutine routine = StartCoroutine(PlayCoroutine(token));
            _runningVFX.Add(token, routine);
            return token;
        }

        /// <summary> Stops a certain played Sound </summary>
        /// <param name="token"> the token received when playing this sound </param>
        protected void StopSound(SoundToken<T> token) {

            if (token == null) return;
            if (!_runningVFX.TryGetValue(token, out Coroutine coroutine)) return;

            try {
                if (coroutine != null) StopCoroutine(coroutine);
                if (token.Source != null) Destroy(token.Source);
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        /// <summary> Stops a certain played Sound, but fade it out instead of ending abruptly. </summary>
        /// <param name="token"> The token received when playing this sound </param>
        protected void StopSoundWithFadeOut(SoundToken<T> token) {
            
            if (token == null) return;
            if (!_runningVFX.TryGetValue(token, out Coroutine coroutine)) return;
            
            if (coroutine != null) StopCoroutine(coroutine);

            if (token.Source != null) StartCoroutine(StopCoroutine(token));
        }

        /// <summary>Coroutine that creates the AudioSource, plays the sound and removes the AudioSource after that</summary>
        /// <param name="token"> the data class containing all information about the to be played vfx </param>
        private IEnumerator PlayCoroutine(SoundToken<T> token) {
            
            // wait the provided delay
            yield return new WaitForSeconds(token.Delay);

            if(SoundManager.Instance != null) token.Source.outputAudioMixerGroup = SoundManager.Instance.EffectsAudioGroup;
            else Debug.LogWarning("No SoundManager in this Scene, you need to add one");
            
            // add SoundSource and play the vfx
            token.Event.Play(token.Source);

            // fade in the volume
            float targetVolume = token.Source.volume;
            
            float timePassed = 0;
            while (timePassed < token.FadeDuration) {
                float progress = timePassed / token.FadeDuration;
                token.Source.volume = Mathf.Lerp(0, targetVolume, progress);
                timePassed += Time.deltaTime;
                yield return null;
            }
            
            // if we loop we dont have to wait until we destroy the source
            if (token.Loop) {
                token.Source.loop = true;
                yield break;
            }
            
            // wait the time of the vfx (subtract fadeIn and fadeOut)
            yield return new WaitForSecondsRealtime(Mathf.Max(token.Source.clip.length - token.FadeDuration * 2, 0));

            yield return StopCoroutine(token);
        }

        private IEnumerator StopCoroutine(SoundToken<T> token) {

            float startVolume = token.Source.volume;
            
            float timePassed = 0;
            while (timePassed < token.FadeDuration) {
                float progress = timePassed / token.FadeDuration;
                token.Source.volume = Mathf.Lerp(startVolume, 0, progress);
                timePassed += Time.deltaTime;
                yield return null;
            }
            
            // destroy the source, since we dont need it anymore
            if(token.Source != null) Destroy(token.Source);
        }

        private void OnDestroy() {
            StopAllCoroutines();
        }
        
    }

}