using System;
using System.Linq;

namespace CGames
{
    [Serializable]
    public class CoinRewardConfig : LootRewardConfig<CoinType>, ICloneable<CoinRewardConfig>
    {
        public CoinRewardConfig Clone()
        {
            return new CoinRewardConfig
            {
                HasRandomRewards = this.HasRandomRewards,
                Reward = this.Reward,
                RewardsPool = RewardsPool.ToList(),
                delayBeforeInstantiatingReward = this.DelayBeforeInstantiatingReward
            };
        }
    }
}