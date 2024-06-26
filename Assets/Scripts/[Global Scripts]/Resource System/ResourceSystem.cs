using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class ResourceSystem : IInitializable
    {
        public List<PlayerUpgradeSO> PlayerUpgradesList { get; private set; }
        public List<RulePageSO> RulePagesList { get; private set; }

        public ResourceSystem()
        {
            PlayerUpgradesList = Resources.LoadAll<PlayerUpgradeSO>("Player Upgrades").ToList();

            RulePagesList = Resources.LoadAll<RulePageSO>("Rule Pages").ToList();
        }

        public void Initialize()
        {
            PlayerUpgradesList.Sort((a, b) => a.UpgradeType.CompareTo(b.UpgradeType));
            
            RulePagesList.Sort((a, b) => a.PageIndex.CompareTo(b.PageIndex));
        }
    }
}