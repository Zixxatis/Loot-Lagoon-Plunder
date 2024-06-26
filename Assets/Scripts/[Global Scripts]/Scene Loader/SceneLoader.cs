using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

using Object = UnityEngine.Object;

namespace CGames
{
    public class SceneLoader : IInitializable
    {
        public event Action OnLoadingStarted;
        public event Action OnLoadingFinished;

        public GameScene CurrentScene => (GameScene)SceneManager.GetActiveScene().buildIndex;
        private GameScene? previouslyPlayedLevel = null;
        private readonly List<GameScene> playableLevels = new()
        {
            GameScene.FirstLevel,
            GameScene.SecondLevel,
            GameScene.ThirdLevel,
            GameScene.FourthLevel,
        };

        private LoadingScreenCanvas loadingScreenCanvas;
        private readonly MonoProxy emptyMono;

        public SceneLoader(MonoProxy emptyMono)
        {
            this.emptyMono = emptyMono;
        }

        public void Initialize() => loadingScreenCanvas = Object.FindObjectOfType<LoadingScreenCanvas>();

        private IEnumerator LoadSceneCoroutine(GameScene nextScene)
        {
            GameScene previousScene = CurrentScene;

            if(playableLevels.Contains(previousScene))
                previouslyPlayedLevel = previousScene;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)nextScene);
            asyncLoad.allowSceneActivation = false;

            OnLoadingStarted?.Invoke();
            loadingScreenCanvas.PlayPreTransitionClip();

            while (loadingScreenCanvas.IsAnimationInProgress && asyncLoad.isDone == false)
            {
                yield return null;
            }

            asyncLoad.allowSceneActivation = true;

            // ? Will wait until scene has changed to new. If current scene if reloaded - will wait one "yr null".
            if(previousScene != nextScene)
            {
                while (CurrentScene == previousScene)
                {
                    yield return null;
                }
            }
            else
                yield return null; 
            
            OnLoadingFinished?.Invoke();

            loadingScreenCanvas = Object.FindObjectOfType<LoadingScreenCanvas>();
            emptyMono.StartCoroutine(loadingScreenCanvas.PlayPostTransitionClip());
        }

        public void LoadMainMenu() => emptyMono.StartCoroutine(LoadSceneCoroutine(GameScene.MainMenu));
        public void LoadShip() => emptyMono.StartCoroutine(LoadSceneCoroutine(GameScene.Ship));
        
        public void LoadRandomLevel()
        {
            GameScene levelToLoad = previouslyPlayedLevel == null? 
                                    playableLevels.GetRandomItem() :
                                    playableLevels.GetRandomUniqueItem((GameScene)previouslyPlayedLevel);
                        
            emptyMono.StartCoroutine(LoadSceneCoroutine(levelToLoad));
        }
    }

    public enum GameScene
    {
        MainMenu,
        FirstLevel,
        SecondLevel,
        ThirdLevel,
        FourthLevel,
        Ship
    }
}