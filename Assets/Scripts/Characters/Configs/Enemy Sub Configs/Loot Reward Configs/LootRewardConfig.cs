using System;
using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public abstract class LootRewardConfig<T> where T : Enum
    {
        [SerializeField] protected bool HasRandomRewards;
        [SerializeField] protected T Reward;
        [SerializeField] protected List<T> RewardsPool;
        [Space]
        [SerializeField] protected float delayBeforeInstantiatingReward;

        public float DelayBeforeInstantiatingReward => delayBeforeInstantiatingReward;

        public T GetRewardType()
        {
            if(HasRandomRewards)
                return RewardsPool.GetRandomItem();
            else
                return Reward;
        }
    }
}