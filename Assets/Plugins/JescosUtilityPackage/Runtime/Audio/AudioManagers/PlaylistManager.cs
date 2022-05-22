using System.Collections;
using System.Collections.Generic;
using Beta.Audio;
using UnityEngine;

namespace Beta.Audio {

    /// <summary> Playlist Function, requires MusicManager </summary>
    /// <remarks> Written by Nikolas </remarks>
    public class PlaylistManager : MonoBehaviour {

        [Tooltip("The Playlist that will be played")]
        [SerializeField] private List<MusicAudioEvent> _playlist;
        
        [Tooltip("If true music is randomly ordered")]
        [SerializeField] private bool _randomShuffle;
        
        [Tooltip("If true music plays automatically")]
        [SerializeField] private bool _autoplay;

        /// <summary> Reference to the PlaylistCoroutine </summary>
        private Coroutine _playRoutine;

        /// <summary> Lenght of the played title - fadeTime </summary>
        private float _playlistPlayTime = 0;

        /// <summary> Moves GameObject to DontDestroyOnLoadScene and finds MusicManager </summary>
        private void Start() {
            if(_autoplay) StartPlaylist();
        }

        /// <summary> Starts the playlist, shuffling through each song </summary>
        public void StartPlaylist() {
            _playRoutine = StartCoroutine(Playlist(MusicManager.Instance.GetFadeTime()));
        }

        /// <summary> Stops Playlist immediately </summary>
        public void EndPlaylist() {
            MusicManager.Instance.StopMusic();
            StopCoroutine(_playRoutine);
        }

        /// <summary> Loops the Playlist </summary>
        private IEnumerator Playlist(float fadeTime) {

            // create Array
            MusicAudioEvent[] array = _playlist.ToArray();
            
            while (true) {

                // random shuffle
                if(_randomShuffle) FisherYates(array);
                
                foreach (MusicAudioEvent m in array) {
                    MusicManager.Instance.PlayMusic(m);
                    _playlistPlayTime = Mathf.Max(m.clip.length - fadeTime, 1f);
                    yield return new WaitForSeconds(_playlistPlayTime);
                }
            }
        }

        /// <summary> Shuffle Algorithm for Musicevent Array</summary>
        /// <param name="array">the array which shall be shuffled</param>
        /// <remarks>Nikolas, Erik</remarks>
        private static void FisherYates(MusicAudioEvent[] array) {
            System.Random r = new System.Random();
            for (int i = array.Length - 1; i > 0; i--) {
                int index = r.Next(i);
                (array[index], array[i]) = (array[i], array[index]);
            }
        }

        private void OnDestroy() {
            StopAllCoroutines();
        }

    }

}
