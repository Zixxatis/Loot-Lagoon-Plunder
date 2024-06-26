using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "Player Upgrade", menuName = "Scriptable Objects/Player/Player Upgrade", order = 0)]
    public class PlayerUpgradeSO : ScriptableObject, ILocalizable
    {
        [field: SerializeField] public UpgradeRarity UpgradeRarity { get; private set; }
        [field: SerializeField] public PlayerUpgradeType UpgradeType { get; private set; }
        [field: Space]
        [field: SerializeField] public LocalizationKeyField TitleKey { get; private set; }
        [field: SerializeField] public LocalizationKeyField DescriptionKey { get; private set; }
        [field: Space]
        [field: SerializeField] public Sprite IconSprite { get; private set; }
        [field: Space]
        [field: SerializeField] public ColoredGems Price { get; private set; }

        public List<LocalizationKeyField> GetAllLocalizationKeys()
        {
            return new()
            {
                TitleKey,
                DescriptionKey
            };
        }
    }
}