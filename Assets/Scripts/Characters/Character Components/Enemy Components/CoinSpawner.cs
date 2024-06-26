using System;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class CoinSpawner : MonoBehaviour
    {
        private CoinRewardConfig coinRewardConfig;
        private Func<Vector3> getSpawnPointPosition;

        private LootFactory lootFactory;

        [Inject]
        private void Construct(LootFactory lootFactory)
        {
            this.lootFactory = lootFactory;
        }

        public void Initialize(CoinRewardConfig coinRewardConfig, Func<Vector3> getSpawnPointPosition)
        {
            this.coinRewardConfig = coinRewardConfig;
            this.getSpawnPointPosition = getSpawnPointPosition;
        }

        public void SpawnLoot() => lootFactory.Spawn(coinRewardConfig, SpawnPointPosition);
        private Vector3 SpawnPointPosition => getSpawnPointPosition == null? this.transform.position : getSpawnPointPosition();
    }
}