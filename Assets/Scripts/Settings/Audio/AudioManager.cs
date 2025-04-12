using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Settings.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private List<Sound> music, sounds, ambient;
        [SerializeField] private AudioResource musicAudioRandomController;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlaySfx(string soundName)
        {
            Sound s = sounds.Find(x => x.name == soundName);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + soundName + " not found!");
                return;
            }
            
            sfxSource.PlayOneShot(s.clip);
        }

        public void PlayMusic(string musicName)
        {
            Sound s = music.Find(x => x.name == musicName);

            if (s == null)
            {
                Debug.LogWarning("Music: " + musicName + " not found!");
                return;
            }
            
            musicSource.clip = s.clip;
            musicSource.Play();
        }

        public void PlayAmbient(string ambientName)
        {
            Sound s = ambient.Find(x => x.name == ambientName);

            if (s == null)
            {
                Debug.LogWarning("Ambient: " + ambientName + " not found!");
                return;
            }

            ambientSource.clip = s.clip;
            ambientSource.Play();
        }
        
        public void StartMusicShuffle()
        {
            musicSource.resource = musicAudioRandomController;
            musicSource.Play();
        }

        public void ShuffleMusic()
        {
            List<Sound> playlist = music;
            int n = playlist.Count;

            if (n < 1)
            {
                Debug.Log("Music is empty!");
                return;
            }

            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                (playlist[k], playlist[n]) = (playlist[n], playlist[k]);
            }

            StartCoroutine(PlayMusicPlaylist(playlist));
        }

        private IEnumerator PlayMusicPlaylist(List<Sound> playlist)
        {
            foreach (var song in playlist)
            {
                PlayMusic(song.name);
                yield return new WaitForSeconds(song.clip.length);
            }
        }
    }
}
