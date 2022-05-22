using Beta.Audio;
using UnityEngine.Audio;

namespace Plugins.Audio.AudioManagers {

    public class VoiceManager : GenericManager<VoiceAudioEvent>{
        protected override AudioMixerGroup Group => SoundManager.Instance.VoicesAudioGroup;

        public void PlaySimpleSFX(VoiceAudioEvent voiceAudioEvent) => PlayVoice(voiceAudioEvent);
        
        /// <summary> Plays the provided Event </summary>
        /// <param name="voiceAudioEvent"> Name of the event, which shall be played </param>
        /// <param name="delay"> the duration between this method call and the start of the vfx </param>
        /// /// <param name="fadeDuration"> the time it takes until the effect is turned up and down </param>
        /// <param name="loop"> if the effect should loop, if true it can only be canceled with the token </param>
        /// <returns> the token that can be used to cancel the vfx </returns>
        public SoundToken<VoiceAudioEvent> PlayVoice(VoiceAudioEvent voiceAudioEvent, float delay = 0, float fadeDuration = 0, bool loop = false) {
            return PlaySound(voiceAudioEvent, delay, fadeDuration, loop);
        }

        /// <summary> Stops a certain played Sound </summary>
        /// <param name="token"> the token received when playing this sound </param>
        public void StopVoice(SoundToken<VoiceAudioEvent> token) => StopSound(token);
    }
}
    