using System;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class GemCollectable : Collectable
    {
        [field: Header("Gem Info")]
        [field: SerializeField] public GemType GemType { get; private set; }

        private Action<ColoredGems> getGemsAction;
        private Action updateGemsInfoAction;

        [Inject]
        private void Construct(PlayerDataView playerDataView, HeadsUpDisplay headsUpDisplay)
        {
            this.getGemsAction = playerDataView.GetGems;
            this.updateGemsInfoAction = headsUpDisplay.UpdateGemsInfo;
        }

        public override void UpdatePlayerCollectables()
        {
            switch (GemType)
            {
                case GemType.Red:
                    getGemsAction?.Invoke(new(1, 0, 0));
                    break;

                case GemType.Green:
                    getGemsAction?.Invoke(new(0, 1, 0));
                    break;

                case GemType.Blue:
                    getGemsAction?.Invoke(new(0, 0, 1));
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public override void UpdateHUD() => updateGemsInfoAction?.Invoke();
    }

    public enum GemType
    {
        Red,
        Green,
        Blue
    }
}