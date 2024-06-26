using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using CGames.Utilities;

namespace CGames
{
    public class TraderPanel : MerchantPanel
    {
        [Space]
        [SerializeField] private TraderSlot redGemSlot;
        [SerializeField] private TraderSlot greenGemSlot;
        [SerializeField] private TraderSlot blueGemSlot;
        [Space]
        [SerializeField] private TraderSlot lesserHealSlot;
        [SerializeField] private TraderSlot greaterHealSlot;
        [Space]
        [SerializeField] private TraderSlot randomGemsSlot;
        [SerializeField] private TraderSlot trioGemsSlot;

        private HealthSystem playerHealthSystem;
        private List<TraderSlot> traderSlots;

        private Action<ColoredGems> getGemsAction;
        
        [Inject]
        private void Construct(PlayerDataView playerDV)
        {
            this.getGemsAction = playerDV.GetGems;
        }
        
        protected override void InitializePanel()
        {
            PrepareSingleSlots();
            PrepareHealSlots();
            PrepareOtherGemSlots();

            traderSlots = new()
            {
                redGemSlot,
                greenGemSlot,
                blueGemSlot,
                lesserHealSlot,
                greaterHealSlot,
                randomGemsSlot,
                trioGemsSlot
            };

            this.playerHealthSystem = FindObjectOfType<PlayerCharacter>().HealthSystem;
        }

        public override void ShowPanel()
        {
            base.ShowPanel();

            HandleAllSlotsInteractivity();
        }

        private void PrepareSingleSlots()
        {
            redGemSlot.InitializeGemSlot
            (
                TraderUtilities.SingleGemPrice,
                delegate
                {
                    getGemsAction?.Invoke(new(1, 0, 0));
                    HandleAllSlotsInteractivity();
                }
            );

            greenGemSlot.InitializeGemSlot
            (
                TraderUtilities.SingleGemPrice, 
                delegate
                {
                    getGemsAction?.Invoke(new(0, 1, 0));
                    HandleAllSlotsInteractivity();
                }
            );

            blueGemSlot.InitializeGemSlot
            (
                TraderUtilities.SingleGemPrice, 
                delegate
                {
                    getGemsAction?.Invoke(new(0, 0, 1));
                    HandleAllSlotsInteractivity();
                }
            );
        }

        private void PrepareHealSlots()
        {
            lesserHealSlot.InitializeHealSlot
            (
                TraderUtilities.LesserHealPrice, 
                delegate
                { 
                    playerHealthSystem.HealInPercents(TraderUtilities.LesserHealRestoreInPercent);
                    HandleAllSlotsInteractivity();
                }, 
                () => playerHealthSystem.HasFullHP,
                $"{TraderUtilities.LesserHealRestoreInPercent}%"
            );

            greaterHealSlot.InitializeHealSlot
            (
                TraderUtilities.GreaterHealPrice,
                delegate
                { 
                    playerHealthSystem.HealInPercents(TraderUtilities.GreaterHealRestoreInPercent);
                    HandleAllSlotsInteractivity();
                },
                () => playerHealthSystem.HasFullHP,
                $"{TraderUtilities.GreaterHealRestoreInPercent}%"
            );
        }
        
        private void PrepareOtherGemSlots()
        {
            randomGemsSlot.InitializeGemSlot
            (
                TraderUtilities.RandomGemsPrice, 
                delegate
                { 
                    getGemsAction?.Invoke(GetTwoRandomGems());
                    HandleAllSlotsInteractivity();
                }, 
                "x2"
            );

            trioGemsSlot.InitializeGemSlot
            (
                TraderUtilities.TrioGemsPrice, 
                delegate
                { 
                    getGemsAction?.Invoke(new(1, 1, 1));
                    HandleAllSlotsInteractivity();
                }, 
                "x3"
            );
        }

        private ColoredGems GetTwoRandomGems()
        {
            ColoredGems coloredGems = new();

            for (int i = 0; i < 2; i++)
            {
                coloredGems[i]++;
            }

            return coloredGems;
        }

        private void HandleAllSlotsInteractivity()
        {
            traderSlots.ForEach(x => x.HandleInteractivity());
            
            SelectFirstInteractableButton();
        }

        private void SelectFirstInteractableButton()
        {
            foreach (var slot in traderSlots)
            {
                if(slot.IsAvailable)
                {
                    slot.SelectButton();
                    return;
                }
            }

            returnButton.Select();
        }
    }
}