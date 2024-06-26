using UnityEngine;
using Zenject;

namespace CGames
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private HeadsUpDisplay headsUpDisplay;
        [Space]
        [SerializeField] private LootFactory lootFactory;
        [Space]
        [SerializeField] private LevelController levelController;

        public override void InstallBindings()
        {
            Container.Bind<HeadsUpDisplay>().FromInstance(headsUpDisplay).AsSingle();
            Container.Bind<LootFactory>().FromInstance(lootFactory).AsSingle();

            Container.BindInterfacesAndSelfTo<LevelController>().FromInstance(levelController).AsSingle();
        }

    #if UNITY_EDITOR
        [ContextMenu("Find All Missing Scripts")]
        private void FindAllScripts()
        {
            if(headsUpDisplay == null)
                headsUpDisplay = FindObjectOfType<HeadsUpDisplay>();

            if(levelController == null)
                levelController = FindObjectOfType<LevelController>();

            if(lootFactory == null)
                lootFactory = FindObjectOfType<LootFactory>();
            
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
    #endif
    
    }
}