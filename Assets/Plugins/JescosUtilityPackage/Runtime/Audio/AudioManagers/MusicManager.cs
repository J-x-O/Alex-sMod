using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Beta.Audio;
using JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween;
using Plugins.Audio.AudioManagers;
using UnityEngine;

namespace Beta.Audio {

    /// <summary> Manages non-adaptive Music </summary>
    /// <remarks> Written by Nikolas </remarks>
    [DefaultExecutionOrder(-1)]
    public class MusicManager : MonoBehaviour {

        public static MusicManager Instance {
            get {
                if (_instance == null) {
                    GameObject manager = new GameObject("Music Manager");
                    MusicManager script = manager.AddComponent<MusicManager>();
                    _instance = script;
                }

                return _instance;
            }
        }

        private static MusicManager _instance;
        
        /// <summary>Duration of blend</summary>
        [Tooltip("Defines how long the blend takes")]
        [SerializeField] private float _fadeTime = 0.5f;

        /// <summary>Stores references to AudioSources</summary>
        private SoundSourceWrapper[] _source = new SoundSourceWrapper[2];

        /// <summary>Shows if there is already music playing</summary>
        private bool _active = false;

        /// <summary>Shows if fade  is in progress</summary>
        private bool _isFading = false;

        /// <summary>Reference to Coroutine</summary>
        private Coroutine _stopCoroutine;

        /// <summary>
        /// Turns array into dictionary, creates AudioSources,
        /// moves gameobject to dontdestroyonload-scene
        /// </summary>
        private void Awake() {

            // note to self: dont access Instance here, this leads to an endless loop
            if (_instance != null) {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            for(int i = 0; i < _source.Length; i++) {
                _source[i] = new SoundSourceWrapper(gameObject, i);
            }
        }

        /// <summary>Starts playing music, starts blend, if music is already playing</summary>
        /// <param name="identifier">name of the music, same as given in the inspector</param>
        /// <param name="continueFromSamePointInTime"> If true, the music track that gets blended in will be synced up with the currently playing track. </param>
        public void PlayMusic(MusicAudioEvent identifier, bool continueFromSamePointInTime = false) {
            if(_active) {
                Blend(identifier, continueFromSamePointInTime);
            }
            else {
                identifier.Play(_source[0].Source);
                _active = true;
            }
        }

        /// <summary>Starts a coroutine to stop music</summary>
        public void StopMusic() {
            _stopCoroutine = StartCoroutine(Stopper());
        }

        /// <summary>Returns fadetime</summary>
        /// <remarks>Nikolas</remarks>
        public float GetFadeTime() => _fadeTime;

        /// <summary>Fades actual music out and fades new music in</summary>
        /// <param name="musicEvent">name of the music, same as given in the inspector</param>
        /// <param name="continueFromSamePointInTime"> If true, the music track that gets blended in will be synced up with the currently playing track. </param>
        private void Blend(MusicAudioEvent musicEvent, bool continueFromSamePointInTime = false) {

            SoundSourceWrapper wrapper = GetAudioSource() ?? ReleaseAudioSource();

            float inVolume = musicEvent.volume;
            wrapper.Source.volume = 0;

            musicEvent.Play(wrapper.Source);

            SoundSourceWrapper wrapperOut = wrapper.RefId == 0 ? _source[1] : _source[0];

            if (continueFromSamePointInTime) {
                wrapper.Source.time = wrapperOut.Source.time;
            }
            
            _isFading = true;

            //FadeOut
            wrapperOut.FadeTween = SmoothBrainTween.Value(wrapperOut.Source.volume, 0, _fadeTime)
                .SetOnUpdate((value) => wrapperOut.Source.volume = value);

            //FadeIn
            wrapper.FadeTween = SmoothBrainTween.Value(0, inVolume, _fadeTime)
                .SetOnUpdate((value) => wrapper.Source.volume = value)
                .SetOnFinish(() => _isFading = false);
        }

        /// <summary>Waits until fade is done, than fades actual playing music out</summary>
        private IEnumerator Stopper() {
            
            while(_isFading) yield return new WaitForSeconds(0.5f);

            int pos = 0;
            for(int i = 0; i < _source.Length; i++) {
                if(_source[i].Source.volume != 0) {
                    pos = i;
                }
            }

            _source[pos].FadeTween = SmoothBrainTween.Value(_source[pos].Source.volume, 0, _fadeTime)
                .SetOnUpdate((value) => _source[pos].Source.volume = value)
                .SetOnFinish(() => {
                    foreach(SoundSourceWrapper wrapper in _source) {
                        wrapper.Source.Stop();
                    }
                    _active = false;
                });

        }

        /// <summary>Returns empty AudioSource, Null if all Sources are used</summary>
        private SoundSourceWrapper GetAudioSource() => _source.FirstOrDefault(wrapper => !wrapper.IsPlaying);

        /// <summary>Stops the AudioSource with the lowest volume and returns it</summary>
        private SoundSourceWrapper ReleaseAudioSource() {
            
            // find the SoundSource with the lower volume
            float lowest = float.MaxValue;
            SoundSourceWrapper min = null;
            foreach (SoundSourceWrapper wrapper in _source) {
                if (wrapper.Source.volume < lowest) {
                    lowest = wrapper.Source.volume;
                    min = wrapper;
                }
            }
            
            // Stop the SoundSource
            if(min != null) min.ReleaseSource();
            else Debug.LogError("Couldn't find lowest Source");

            return min;
        }

        public void ImmediatelyStopMusic() {
            foreach (SoundSourceWrapper soundSourceWrapper in _source) {
                soundSourceWrapper.ReleaseSource();
            }
        }

        private void OnDestroy() {
            StopAllCoroutines();
        }
    }

}
