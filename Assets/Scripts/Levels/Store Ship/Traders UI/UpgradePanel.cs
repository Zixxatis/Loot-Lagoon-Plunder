using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class UpgradePanel : MerchantPanel
    {

        [Header("General - Elements")]
        [SerializeField] private GameObject storeSlotsPanelObject;
        [SerializeField] private GameObject playerSlotsPanelObject;
        [Space]
        [SerializeField] private Button swapButton;
        [SerializeField] private TextLocalizer swapButtonLTMP;

        [Header("Store Slots - Elements")]
        [SerializeField] private UpgradeStoreSlot firstStoreSlot;
        [SerializeField] private UpgradeStoreSlot secondStoreSlot;
        [Space]
        [SerializeField] private GameObject emptyStoreNotificationObject;

        [Header("Player Upgrade - Elements")]
        [SerializeField] private Transform scrollViewContentTransform;
        [SerializeField] private PlayerUpgradeSlot playerUpgradeSlotPrefab;
        [Space]
        [SerializeField] private GameObject emptyPlayerNotificationObject;
        
        private readonly List<PlayerUpgradeSlot> playerUpgradeSlotsObjectsList = new();
        private PlayerUpgradeSlot firstShopUpgradePlayerSlot;
        private PlayerUpgradeSlot secondShopUpgradePlayerSlot;

        private PlayerDataView playerDV;
        private ResourceSystem resourceSystem;
        
        [Inject]
        private void Construct(PlayerDataView playerDV, ResourceSystem resourceSystem)
        {
            this.playerDV = playerDV;
            this.resourceSystem = resourceSystem;
        }

        protected override void InitializePanel()
        {
            swapButton.onClick.AddListener(SwapPanels);
            playerSlotsPanelObject.DeactivateObject();
            UpdateSwapButtonText();

            InitializeStoreSlots();
            CreatePlayerSlots();
        }

        public override void ShowPanel()
        {
            base.ShowPanel();

            firstStoreSlot.HandleInteractivity();
            secondStoreSlot.HandleInteractivity();

            SelectFirstInteractableButton();

            if(playerDV.HasAllUpgradesUnlocked() && storeSlotsPanelObject.activeInHierarchy)
                SwapPanels();
        }

        private void InitializeStoreSlots()
        {
            List<PlayerUpgradeType> lockedUpgradesList = playerDV.UpgradesDictionary.Where(kvp => kvp.Value == false)
                                                                                    .Select(kvp => kvp.Key)
                                                                                    .ToList();

            if (lockedUpgradesList.Count == 0)
            {
                firstStoreSlot.DeactivateGameObject();
                secondStoreSlot.DeactivateGameObject();

                emptyStoreNotificationObject.ActivateObject();
                return;
            }
            else
                emptyStoreNotificationObject.DeactivateObject();

            PrepareFirstStoreSlot(lockedUpgradesList);

            if (lockedUpgradesList.Count > 0)
                PrepareSecondStoreSlot(lockedUpgradesList);
            else
                secondStoreSlot.DeactivateGameObject();
        }

        private void PrepareFirstStoreSlot(List<PlayerUpgradeType> lockedUpgradesList)
        {
            PlayerUpgradeType firstUpgradeType = lockedUpgradesList.GetRandomItem();
            PlayerUpgradeSO firstUpgradeSO = resourceSystem.PlayerUpgradesList.First(x => x.UpgradeType == firstUpgradeType);

            firstShopUpgradePlayerSlot = Instantiate(playerUpgradeSlotPrefab, scrollViewContentTransform);
            firstShopUpgradePlayerSlot.Initialize(firstUpgradeSO);
            firstShopUpgradePlayerSlot.DeactivateGameObject();
            playerUpgradeSlotsObjectsList.Add(firstShopUpgradePlayerSlot);

            firstStoreSlot.Initialize
            (
                firstUpgradeSO,
                delegate
                {
                    playerDV.UnlockUpgrade(firstUpgradeType);
                    secondStoreSlot.HandleInteractivity();

                    firstShopUpgradePlayerSlot.ActivateGameObject();
                    SortPlayerSlots();
                    
                    if(emptyPlayerNotificationObject.activeSelf)
                        emptyPlayerNotificationObject.DeactivateObject();

                    SelectFirstInteractableButton();
                }
            );

            lockedUpgradesList.Remove(firstUpgradeType);
        }

        private void PrepareSecondStoreSlot(List<PlayerUpgradeType> lockedUpgradesList)
        {
            PlayerUpgradeType secondUpgradeType = lockedUpgradesList.GetRandomItem();
            PlayerUpgradeSO secondUpgradeSO = resourceSystem.PlayerUpgradesList.First(x => x.UpgradeType == secondUpgradeType);

            secondShopUpgradePlayerSlot = Instantiate(playerUpgradeSlotPrefab, scrollViewContentTransform);
            secondShopUpgradePlayerSlot.Initialize(secondUpgradeSO);
            secondShopUpgradePlayerSlot.DeactivateGameObject();
            playerUpgradeSlotsObjectsList.Add(secondShopUpgradePlayerSlot);

            secondStoreSlot.Initialize
            (
                secondUpgradeSO,
                delegate
                {   
                    playerDV.UnlockUpgrade(secondUpgradeType);
                    firstStoreSlot.HandleInteractivity();
                    
                    secondShopUpgradePlayerSlot.ActivateGameObject();
                    SortPlayerSlots();

                    if(emptyPlayerNotificationObject.activeSelf)
                        emptyPlayerNotificationObject.DeactivateObject();

                    SelectFirstInteractableButton();
                }
            );

            lockedUpgradesList.Remove(secondUpgradeType);
        }

        private void CreatePlayerSlots()
        {
            if(playerDV.HasAllUpgradesLocked())
            {
                emptyPlayerNotificationObject.ActivateObject();
                return;
            }
            else
                emptyPlayerNotificationObject.DeactivateObject();

            List<PlayerUpgradeType> unlockedUpgradesList = playerDV.UpgradesDictionary.Where(kvp => kvp.Value == true)
                                                                                    .Select(kvp => kvp.Key)
                                                                                    .ToList();

            foreach (PlayerUpgradeType upgradeType in unlockedUpgradesList)
            {
                PlayerUpgradeSlot upgradeSlot = Instantiate(playerUpgradeSlotPrefab, scrollViewContentTransform);
                PlayerUpgradeSO upgradeSO = resourceSystem.PlayerUpgradesList.First(x => x.UpgradeType == upgradeType);

                upgradeSlot.Initialize(upgradeSO);
                playerUpgradeSlotsObjectsList.Add(upgradeSlot);
            }

            SortPlayerSlots();
        }

        private void SortPlayerSlots()
        {
            playerUpgradeSlotsObjectsList.Sort((x, y) => x.UpgradeRarity.CompareTo(y.UpgradeRarity));

            for (int i = 0; i < playerUpgradeSlotsObjectsList.Count; i++)
            {
                playerUpgradeSlotsObjectsList[i].transform.SetSiblingIndex(i);
            }
        }

        private void SwapPanels()
        {
            storeSlotsPanelObject.SetActive(!storeSlotsPanelObject.activeInHierarchy);
            playerSlotsPanelObject.SetActive(!storeSlotsPanelObject.activeInHierarchy);

            UpdateSwapButtonText();
        }

        private void UpdateSwapButtonText()
        {
            if (storeSlotsPanelObject.activeInHierarchy)
                swapButtonLTMP.SetKeyAndUpdate("Upgrader_SwapToPlayer");
            else
                swapButtonLTMP.SetKeyAndUpdate("Upgrader_SwapToTrades");
        }

        private void SelectFirstInteractableButton()
        {
            if(firstStoreSlot.IsAvailable)
            {   
                firstStoreSlot.SelectButton();
                return;
            }

            if(secondStoreSlot.IsAvailable)
            {   
                secondStoreSlot.SelectButton();
                return;
            }

            swapButton.Select();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            swapButton.onClick.RemoveListener(SwapPanels);
        }
    }
}