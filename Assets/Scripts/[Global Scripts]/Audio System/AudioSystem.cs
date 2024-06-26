using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class AudioSystem : MonoBehaviour,  IInitializable
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource mainEffectsSource;
        [SerializeField] private AudioSource oneShotSource;

        [Header("Songs")]
        [SerializeField] private AudioClip mainMenuSong;
        [SerializeField] private AudioClip shipSong;
        [SerializeField] private List<AudioClip> levelSongsList;

        private SceneLoader sceneLoader;
        private ConfigDataView configDV;

        [Inject]
        private void Construct(SceneLoader sceneLoader, ConfigDataView configDV)
        {
            this.sceneLoader = sceneLoader;
            this.configDV = configDV;
        }

        public void Initialize()
        {
            configDV.OnVolumeChanged += UpdateMusicVolume;

            sceneLoader.OnLoadingFinished += SetSceneSong;
            SetSceneSong();

            oneShotSource.volume = configDV.EffectsVolume;
            oneShotSource.loop = false;
        }

        public void SetSceneSong()
        {
            switch (sceneLoader.CurrentScene)
            {
                case GameScene.MainMenu:
                    PlayMusic(mainMenuSong);
                    break;

                case GameScene.Ship:
                    PlayMusic(shipSong);
                    break;
                    
                default:
                    PlayMusic(levelSongsList.GetRandomItem());
                    break;
            }
        }

        private void PlayMusic(AudioClip audioClip)
        {
            musicSource.clip = audioClip;
            musicSource.volume = configDV.MusicVolume;
            musicSource.loop = true;
            
            musicSource.Play();
        }

        public void UpdateMusicVolume()
        {
            if(musicSource.isPlaying)
                musicSource.volume = configDV.MusicVolume;
        }

        public void PlaySFX(AudioClip audioClip, bool shouldSetOnLoop = false)
        {
            mainEffectsSource.clip = audioClip;
            mainEffectsSource.volume = configDV.EffectsVolume;
            mainEffectsSource.loop = shouldSetOnLoop;

            mainEffectsSource.Play();
        }

        public void StopSFX()
        {
            if(mainEffectsSource.clip == null)
                return;

            mainEffectsSource.Stop();
        } 

        public void PlayOneShotSFX(AudioClip audioClip) => oneShotSource.PlayOneShot(audioClip, configDV.EffectsVolume);

        private void OnDestroy()
        {
            configDV.OnVolumeChanged -= UpdateMusicVolume;
            sceneLoader.OnLoadingFinished -= SetSceneSong;
        }
    }
}