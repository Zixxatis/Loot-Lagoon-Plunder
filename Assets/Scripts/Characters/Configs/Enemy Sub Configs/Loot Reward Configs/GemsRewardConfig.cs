using System;
using System.Linq;

namespace CGames
{
    [Serializable]
    public class GemsRewardConfig : LootRewardConfig<GemType>, ICloneable<GemsRewardConfig>
    {
        public GemsRewardConfig Clone()
        {
            return new GemsRewardConfig
            {
                HasRandomRewards = this.HasRandomRewards,
                Reward = this.Reward,
                RewardsPool = RewardsPool.ToList(),
                delayBeforeInstantiatingReward = this.DelayBeforeInstantiatingReward
            };
        }
    }
}