using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGames.Utilities;
using Zenject;

namespace CGames
{
    public abstract class EnemyNPC : Character
    {
        [field: Header("Enemy NPC Components")]
        [field: SerializeField] protected EnemyBrain EnemyBrain { get; private set; }
        [field: SerializeField] protected DialogueAnimator DialogueAnimator { get; private set; }
        [Space]
        [SerializeField] private TouchContactDetector touchContactDetector;
        [field: SerializeField] public WallsDetector WallsDetector { get; private set; }
        [Space]
        [SerializeField] private CoinSpawner lootSpawner;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public EnemyAttributes EnemyAttributes => (EnemyAttributes)CharacterAttributes;
        protected abstract float DeathAnimationDuration { get; }

        private LevelController levelController;
        
        [Inject]
        private void Construct(LevelController levelController)
        {
            this.levelController = levelController;
        }

        protected override CharacterAttributes GetCharacterAttributes(CharacterConfig characterConfig) => new EnemyAttributes((EnemyConfig)characterConfig);
        protected override abstract List<IState> GetCharacterStates();
        protected override void InitializeCharacter()
        {
            DialogueAnimator.Initialize();
            EnemyBrain.Initialize
            (
                EnemyAttributes.DetectionConfig,
                GetDefaultTaskList(),
                GetLongRangeTaskList(),
                GetCloseRangeTaskList(),
                EnemyAttributes.DetectionConfig.ShouldUseDefaultDialogueBehaviour ? DialogueAnimator : null
            );

            touchContactDetector.Initialize(EnemyAttributes.TouchKnockBackConfig);
            WallsDetector.Initialize();

            lootSpawner.Initialize(EnemyAttributes.CoinRewardConfig, () => Body.transform.position);

            levelController.OnPlayerDeathAction += ReturnToDefaultTaskAfterPlayerDeath;

            InitializeEnemy();
        }

        protected abstract Queue<ITask> GetDefaultTaskList();
        protected virtual Queue<ITask> GetLongRangeTaskList() => null;
        protected virtual Queue<ITask> GetCloseRangeTaskList() => null;

        protected abstract void InitializeEnemy();
        protected abstract void EnterAttackingState();

        public void ChangeDirection() => transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        
        private void ReturnToDefaultTaskAfterPlayerDeath() 
        {
            EnemyBrain.LoseAllTargets();
            EnemyBrain.DisableDetectors();
        }

        private IEnumerator RemoveCorpse()
        {     
            spriteRenderer.sortingLayerName = LayerUtilities.DestroyedSortingLayerName;

            float timer = 0f;

            while(timer < DeathAnimationDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            this.gameObject.DeactivateObject();
            Destroy(this.gameObject);
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            CharacterData.ResetInput();
            CharacterData.ExitAirborne();
            CharacterData.ExitAttack();

            touchContactDetector.DeactivateGameObject();

            EnemyBrain.DisableDetectors();
            EnemyBrain.DeactivateGameObject();

            DialogueAnimator.PlayDialogueAnimation(DialogueEmotion.Death);

            StartCoroutine(RemoveCorpse());
            lootSpawner.SpawnLoot();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            levelController.OnPlayerDeathAction -= ReturnToDefaultTaskAfterPlayerDeath;
        }
    }
}