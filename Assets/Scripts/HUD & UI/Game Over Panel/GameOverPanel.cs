using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using System;

namespace CGames
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private Image blockerImage;
        [Space]
        [SerializeField] private TextMeshProUGUI coinsLTMP;
        [SerializeField] private TextMeshProUGUI redGemsAmountTMP;
        [SerializeField] private TextMeshProUGUI greenGemsAmountTMP;
        [SerializeField] private TextMeshProUGUI blueGemsAmountTMP;
        [Space]
        [SerializeField] private Button quitButton;
        [Space]
        [SerializeField] private AudioClip gameOverAudioClip;

        private SceneLoader sceneLoader;
        private Action<AudioClip> playOneShotEffectAction;

        [Inject]
        private void Construct(SceneLoader sceneLoader, AudioSystem audioSystem)
        {
            this.sceneLoader = sceneLoader;
            this.playOneShotEffectAction = audioSystem.PlayOneShotSFX;
        }

        private void Awake()
        {
            quitButton.onClick.AddListener(sceneLoader.LoadMainMenu);

            blockerImage.DeactivateGameObject();
            this.DeactivateGameObject();
        }

        public void ShowPanel(int coinsLost, ColoredGems gemsAmount)
        {
            this.ActivateGameObject();
            blockerImage.ActivateGameObject();

            coinsLTMP.text = coinsLost.ToString();
            redGemsAmountTMP.text = gemsAmount.Red.ToString();
            greenGemsAmountTMP.text = gemsAmount.Green.ToString();
            blueGemsAmountTMP.text = gemsAmount.Blue.ToString();

            playOneShotEffectAction?.Invoke(gameOverAudioClip);

            quitButton.Select();
        }

        private void OnDestroy() => quitButton.onClick.RemoveListener(sceneLoader.LoadMainMenu);
    }
}