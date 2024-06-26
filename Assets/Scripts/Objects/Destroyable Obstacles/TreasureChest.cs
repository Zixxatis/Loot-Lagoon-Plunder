using System;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class TreasureChest : DestroyableObstacle
    {
        [Header("Treasure Chest Settings")]
        [SerializeField] private GemsRewardConfig gemsRewardConfig;

        private Action setAsCompletedAction;
        private LootFactory lootFactory;

        [Inject]
        private void Construct(LevelController levelController, LootFactory lootFactory)
        {
            this.setAsCompletedAction = levelController.SetAsCompleted;
            this.lootFactory = lootFactory;
        }

        private void OnEnable() => OnObjectHit += SpawnLoot;

        public void SpawnLoot()
        {
            lootFactory.Spawn(gemsRewardConfig, this.transform.position);

            setAsCompletedAction?.Invoke();
        }

        private void OnDisable() => OnObjectHit -= SpawnLoot;
    }
}