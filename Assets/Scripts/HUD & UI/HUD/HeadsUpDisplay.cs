using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class HeadsUpDisplay : MonoBehaviour
    {
        [Header("Loot Info")]
        [SerializeField] private TextMeshProUGUI coinsValueTMP;
        [Space]
        [SerializeField] private TextMeshProUGUI redGemsValueTMP;
        [SerializeField] private TextMeshProUGUI greenGemsValueTMP;
        [SerializeField] private TextMeshProUGUI blueGemsValueTMP;

        [Header("Health Info")]
        [SerializeField] private Slider healthSlider;
        [Space]
        [SerializeField] private TextMeshProUGUI healthValue;

        private Func<int> getCurrentHealth;
        private Func<int> getMaxHealth;

        private PlayerDataView playerDV;

        [Inject]
        private void Construct(PlayerDataView playerDataView)
        {
            playerDV = playerDataView;
        }

        private void Start() => UpdateCoinsAndGems();

        public void SetupHealthInfo(Func<int> getMaxHealth, Func<int> getCurrentHealth)
        {
            this.getCurrentHealth = getCurrentHealth;
            this.getMaxHealth = getMaxHealth;

            healthSlider.wholeNumbers = true;
            healthSlider.minValue = 0;

            UpdateHealthInfoFully();
        }

        public void UpdateHealthInfo()
        {
            healthSlider.value = getCurrentHealth();
            healthValue.text = getCurrentHealth().ToString();
        }

        private void UpdateHealthInfoFully()
        {
            healthSlider.maxValue = getMaxHealth();
            UpdateHealthInfo();
        }

        public void UpdateCoinsInfo()
        {
            coinsValueTMP.text = playerDV.CoinsAmount.ToString();
        }

        public void UpdateGemsInfo()
        {
            redGemsValueTMP.text = playerDV.ColoredGems.Red.ToString();
            greenGemsValueTMP.text = playerDV.ColoredGems.Green.ToString();
            blueGemsValueTMP.text = playerDV.ColoredGems.Blue.ToString();
        }

        public void UpdateCoinsAndGems()
        {
            UpdateCoinsInfo();
            UpdateGemsInfo();
        }

        public void UpdateAll()
        {
            UpdateHealthInfoFully();
            UpdateCoinsAndGems();
        }
    }
}