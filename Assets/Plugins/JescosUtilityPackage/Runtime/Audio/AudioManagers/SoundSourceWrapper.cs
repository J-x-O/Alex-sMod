using JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween;
using UnityEngine;

namespace Plugins.Audio.AudioManagers {

    public class SoundSourceWrapper {
    
        public int RefId { get; }

        public AudioSource Source { get; private set; }

        public TweenInfo FadeTween;

        public bool IsPlaying => Source.isPlaying;

        public SoundSourceWrapper(GameObject source, int refId) {
            RefId = refId;
            Source = source.AddComponent<AudioSource>();
            Source.outputAudioMixerGroup = SoundManager.Instance != null ? SoundManager.Instance.MusicAudioGroup : null;
            Source.volume = 0;
        }

        /// <summary> Resets the audio source so this wrapper can play something again </summary>
        public void ReleaseSource() {
            if (FadeTween is {Running: true}) SmoothBrainTween.Cancel(FadeTween);
            Source.Stop();
            Source.volume = 0;
        }
    }

}
