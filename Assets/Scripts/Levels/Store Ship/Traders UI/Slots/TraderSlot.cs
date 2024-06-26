using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace CGames
{
    public class TraderSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemValue;
        [SerializeField] private TextMeshProUGUI priceValue;
        [Space]
        [SerializeField] private Button tradeButton;
        [SerializeField] private GameObject soldText;

        public bool IsAvailable => tradeButton.gameObject.activeInHierarchy && tradeButton.interactable;

        private PlayerDataView playerDV;
        private Action updateCoinsAndGemsAction;

        private int priceInCoins;
        private Action postTradeAction;
        private Func<bool> isPlayerHealthFull;

        [Inject]
        private void Construct(PlayerDataView playerDV, HeadsUpDisplay headsUpDisplay)
        {
            this.playerDV = playerDV;
            this.updateCoinsAndGemsAction = headsUpDisplay.UpdateCoinsAndGems;
        }

        public void InitializeGemSlot(int priceInCoins, Action postTradeAction, string itemValueText = null)
        {
            Initialize(priceInCoins, postTradeAction, null, itemValueText);
        }

        public void InitializeHealSlot(int priceInCoins, Action postTradeAction, Func<bool> isPlayerHealthFull, string itemValueText = null)
        {
            Initialize(priceInCoins, postTradeAction, isPlayerHealthFull, itemValueText);
        }

        private void Initialize(int priceInCoins, Action postTradeAction, Func<bool> isPlayerHealthFull, string itemValueText = null)
        {
            this.priceInCoins = priceInCoins;
            this.postTradeAction = postTradeAction;
            this.isPlayerHealthFull = isPlayerHealthFull;

            priceValue.text = priceInCoins.ToString();

            tradeButton.onClick.AddListener(Trade);

            if (itemValue != null && string.IsNullOrEmpty(itemValueText) == false)
                itemValue.text = itemValueText;

            soldText.DeactivateObject();
        }

        public void HandleInteractivity()
        {
            if(isPlayerHealthFull != null && isPlayerHealthFull())
            {
                tradeButton.DisableInteractivityWithText();
                return;
            }

            tradeButton.ChangeInteractivityWithText(playerDV.CoinsAmount >= priceInCoins);
        }

        private void Trade()
        {
            SwapToSoldOut();
            playerDV.SpendCoin(priceInCoins);
            postTradeAction?.Invoke();
            updateCoinsAndGemsAction?.Invoke();
        }

        private void SwapToSoldOut()
        {
            tradeButton.DeactivateGameObject();
            soldText.ActivateObject();
        }

        public void SelectButton() => tradeButton.Select();

        private void OnDestroy() => tradeButton.onClick.RemoveListener(Trade);
    }
}