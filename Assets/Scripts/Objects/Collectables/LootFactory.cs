using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class LootFactory : MonoBehaviour
    {
        [Header("Coins")]
        [SerializeField] private CoinCollectable silverCoin;
        [SerializeField] private CoinCollectable goldCoin;

        [Header("Gems")]
        [SerializeField] private GemCollectable redGem;
        [SerializeField] private GemCollectable greenGem;
        [SerializeField] private GemCollectable blueGem;
        
        private IInstantiator diContainer;

        [Inject]
        private void Construct(IInstantiator diContainer)
        {
            this.diContainer = diContainer;
        }

        public void Spawn<T>(LootRewardConfig<T> lootRewardConfig, Vector3 spawnPointPosition) where T : Enum
        {
            Collectable collectable = lootRewardConfig switch
            {
                CoinRewardConfig coinRewardConfig => GetCoin(coinRewardConfig.GetRewardType()),
                GemsRewardConfig gemsRewardConfig => GetGem(gemsRewardConfig.GetRewardType()),
                _ => throw new ArgumentOutOfRangeException(),
            };

            SpawnCollectable(collectable, spawnPointPosition, lootRewardConfig.DelayBeforeInstantiatingReward);
        }

        private void SpawnCollectable(Collectable collectable, Vector3 spawnPointPosition, float delayBeforeInstantiatingReward)
        {
            if(delayBeforeInstantiatingReward == 0f)
                ChangeCollectablePosition(collectable, spawnPointPosition);
            else
                StartCoroutine(SpawnAfterDelay(collectable, spawnPointPosition, delayBeforeInstantiatingReward));
        }

        private CoinCollectable GetCoin(CoinType coinType)
        {
            return coinType switch
            {
                CoinType.Silver => diContainer.InstantiatePrefabForComponent<CoinCollectable>(silverCoin),
                CoinType.Gold => diContainer.InstantiatePrefabForComponent<CoinCollectable>(goldCoin),
                _ => throw new ArgumentOutOfRangeException(nameof(coinType)),
            };
        }

        private GemCollectable GetGem(GemType gemType)
        {
            return gemType switch
            {
                GemType.Red => diContainer.InstantiatePrefabForComponent<GemCollectable>(redGem),
                GemType.Green => diContainer.InstantiatePrefabForComponent<GemCollectable>(greenGem),
                GemType.Blue => diContainer.InstantiatePrefabForComponent<GemCollectable>(blueGem),
                _ => throw new ArgumentOutOfRangeException(nameof(gemType)),
            };
        }

        private IEnumerator SpawnAfterDelay(Collectable collectable, Vector3 spawnPointPosition, float spawnDelay)
        {
            float timer = 0f;

            collectable.DeactivateGameObject();

            while(timer < spawnDelay)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            ChangeCollectablePosition(collectable, spawnPointPosition);
            collectable.ActivateGameObject();
        }
        
        private void ChangeCollectablePosition(Collectable collectable, Vector3 spawnPointPosition) => collectable.transform.position = spawnPointPosition;
    }
}