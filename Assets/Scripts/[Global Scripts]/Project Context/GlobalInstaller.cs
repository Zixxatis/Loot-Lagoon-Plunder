using UnityEngine;
using Zenject;

namespace CGames
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private MonoProxy monoProxy;
        [Space]
        [SerializeField] private AudioSystem audioSystem;

        public override void InstallBindings()
        {
            Container.Bind<MonoProxy>().FromInstance(monoProxy).AsSingle().NonLazy();

            BindAndLoadResources();
            BindAndLoadData();
            BindAndInitializeSystems();
            BindAndInitializePlayerInputHandler();
        }

        private void BindAndLoadResources()
        {
            Container.BindInterfacesAndSelfTo<ResourceSystem>().AsSingle().NonLazy();
        }

        private void BindAndLoadData()
        {
            Container.BindInterfacesAndSelfTo<ConfigDataView>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerDataView>().AsSingle().NonLazy();
        }

        private void BindAndInitializeSystems()
        {
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LocalizationSystem>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<AudioSystem>().FromInstance(audioSystem).AsSingle().NonLazy();
        }

        private void BindAndInitializePlayerInputHandler()
        {
            Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle().NonLazy();
        }
    }
}