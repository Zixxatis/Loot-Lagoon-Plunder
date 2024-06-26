using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    public class PlayerUpgradeSlot : MonoBehaviour
    {
        [SerializeField] private Image upgradeIcon;
        [Space]
        [SerializeField] private TextLocalizer headerLTMP;
        [SerializeField] private TextLocalizer descriptionLTMP;

        public UpgradeRarity UpgradeRarity { get; private set;}

        public void Initialize(PlayerUpgradeSO playerUpgradeSO)
        {
            this.UpgradeRarity = playerUpgradeSO.UpgradeRarity;

            upgradeIcon.sprite = playerUpgradeSO.IconSprite;

            headerLTMP.SetKeyAndUpdate(playerUpgradeSO.TitleKey);
            descriptionLTMP.SetKeyAndUpdate(playerUpgradeSO.DescriptionKey);
        }
    }
}