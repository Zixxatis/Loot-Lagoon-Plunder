using System;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class CoinCollectable : Collectable
    {
        [field: Header("Coin Info")]
        [field: SerializeField] public CoinType CoinType { get; private set; }
        [SerializeField] private int rewardAmount;

        private Action collectCoinAction;
        private Action updateCoinsInfoAction;

        [Inject]
        private void Construct(PlayerDataView playerDataView, HeadsUpDisplay headsUpDisplay)
        {
            this.collectCoinAction = () => playerDataView.CollectCoin(rewardAmount);
            this.updateCoinsInfoAction = headsUpDisplay.UpdateCoinsInfo;
        }

        public override void UpdatePlayerCollectables() => collectCoinAction?.Invoke();
        public override void UpdateHUD() => updateCoinsInfoAction?.Invoke();
    }

    public enum CoinType
    {
        Silver,
        Gold
    }
}