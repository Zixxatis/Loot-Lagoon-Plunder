using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class UpgradeStoreSlot : MonoBehaviour
    {
        [SerializeField] private Image upgradeIcon;
        [Space]
        [SerializeField] private TextLocalizer headerLTMP;
        [SerializeField] private TextLocalizer descriptionLTMP;
        [Space]
        [SerializeField] private GameObject redGemsObject;
        [SerializeField] private GameObject greenGemsObject;
        [SerializeField] private GameObject blueGemsObject;
        [Space]
        [SerializeField] private TextMeshProUGUI redGemsValueTMP;
        [SerializeField] private TextMeshProUGUI greenGemsValueTMP;
        [SerializeField] private TextMeshProUGUI blueGemsValueTMP;
        [Space]
        [SerializeField] private Button tradeButton;
        [SerializeField] private GameObject soldText;

        private PlayerDataView playerDV;
        private HeadsUpDisplay headsUpDisplay;
        private Action<PlayerUpgradeType> updateStatusBarAction;
        
        [Inject]
        private void Construct(PlayerDataView playerDV, HeadsUpDisplay headsUpDisplay, StatusBarPanel statusBarPanel)
        {
            this.playerDV = playerDV;
            this.headsUpDisplay = headsUpDisplay;
            this.updateStatusBarAction = (x) => statusBarPanel.DisplayNewUpgrade(x);
        }

        public bool IsAvailable => tradeButton.gameObject.activeInHierarchy && tradeButton.interactable;
        private PlayerUpgradeSO playerUpgradeSO;
        private Action postPurchaseAction;

        public void Initialize(PlayerUpgradeSO playerUpgradeSO, Action postPurchaseAction)
        {
            this.playerUpgradeSO = playerUpgradeSO;
            this.postPurchaseAction = postPurchaseAction;

            SetupUpgradeDetails();
            SetPrice();

            tradeButton.onClick.AddListener(Trade);
            soldText.DeactivateObject();
        }

        private void SetupUpgradeDetails()
        {
            upgradeIcon.sprite = playerUpgradeSO.IconSprite;
            headerLTMP.SetKeyAndUpdate(playerUpgradeSO.TitleKey);
            descriptionLTMP.SetKeyAndUpdate(playerUpgradeSO.DescriptionKey);
        }

        private void SetPrice()
        {
            ColoredGems price = playerUpgradeSO.Price;

            if(price.Red == 0)
                redGemsObject.DeactivateObject();
            else
                redGemsValueTMP.text = price.Red.ToString();

            if(price.Green == 0)
                greenGemsObject.DeactivateObject();
            else
                greenGemsValueTMP.text = price.Green.ToString();

            if(price.Blue == 0)
                blueGemsObject.DeactivateObject();
            else
                blueGemsValueTMP.text = price.Blue.ToString();
        }

        public void HandleInteractivity()
        {
            if(tradeButton.gameObject.activeInHierarchy == false)
                return;

            tradeButton.ChangeInteractivityWithText(playerDV.ColoredGems.IsEnoughFor(playerUpgradeSO.Price));
        }

        private void Trade()
        {
            SwapToSoldOut();
            playerDV.SpendGems(playerUpgradeSO.Price);
            FindObjectOfType<PlayerCharacter>().PlayerAttributes.EnableUpgrade(playerUpgradeSO.UpgradeType);
            postPurchaseAction?.Invoke();
            
            headsUpDisplay.UpdateAll();
            updateStatusBarAction.Invoke(playerUpgradeSO.UpgradeType);
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