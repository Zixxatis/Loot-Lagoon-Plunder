using UnityEngine;
using Zenject;

namespace CGames
{
    public class StoreShipInstaller : MonoInstaller
    {
        [SerializeField] private HeadsUpDisplay headsUpDisplay;
        [SerializeField] private StatusBarPanel statusBarPanel;
        [Space]
        [SerializeField] private GoalTracker goalTracker;
        [Space]
        [SerializeField] private StoreShipController storeShipController;

        public override void InstallBindings()
        {
            Container.Bind<HeadsUpDisplay>().FromInstance(headsUpDisplay).AsSingle();
            Container.Bind<StatusBarPanel>().FromInstance(statusBarPanel).AsSingle();

            Container.Bind<GoalTracker>().FromInstance(goalTracker).AsSingle();

            Container.BindInterfacesAndSelfTo<StoreShipController>().FromInstance(storeShipController).AsSingle();
        }

    #if UNITY_EDITOR
        [ContextMenu("Find All Missing Scripts")]
        private void FindAllScripts()
        {
            if(headsUpDisplay == null)
                headsUpDisplay = FindObjectOfType<HeadsUpDisplay>();

            if(statusBarPanel == null)
                statusBarPanel = FindObjectOfType<StatusBarPanel>();

            if(goalTracker == null)
                goalTracker = FindObjectOfType<GoalTracker>();

            if(storeShipController == null)
                storeShipController = FindObjectOfType<StoreShipController>();

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
    #endif
    
    }
}