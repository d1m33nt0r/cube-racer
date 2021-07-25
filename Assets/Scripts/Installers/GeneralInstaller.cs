using Services.DiamondCountManager;
using Services.LevelProgressManager;
using UI.GlobalUI.DiamondCounter;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GeneralInstaller : MonoInstaller
    {
        [SerializeField] private GameObject fpsCounter;
        [SerializeField] private DiamondUI diamondUI;

        // services
        private DiamondCountManager diamondCountManager;
        private LevelProgressManager levelProgressManager;
        private SceneLoader sceneLoader;

        private GameObject debugParent;
        private GameObject servicesParent;

        public override void InstallBindings()
        {
            debugParent = CreateParent("Debug");
            servicesParent = CreateParent("Services");

            diamondCountManager = new DiamondCountManager();
            levelProgressManager = new LevelProgressManager();
            sceneLoader = new SceneLoader();

            BindDiamondCountManager();
            BindLevelProgressManager();
            BindSceneLoader();

            
            BindDiamondUI();
            InstantiateFpsCounter(debugParent);
        }

        private void BindSceneLoader()
        {
            Container
                .Bind<SceneLoader>()
                .FromInstance(sceneLoader)
                .AsSingle();
        }

        private void BindDiamondUI()
        {
            var diamondUI = Container.InstantiatePrefab(this.diamondUI);

            Container
                .Bind<DiamondUI>()
                .FromInstance(diamondUI.GetComponent<DiamondUI>())
                .AsSingle();

            diamondUI.transform.SetParent(servicesParent.transform);
        }

        private void BindDiamondCountManager()
        {
            Container
                .Bind<DiamondCountManager>()
                .FromInstance(diamondCountManager)
                .AsSingle();
        }

        private void BindLevelProgressManager()
        {
            Container
                .Bind<LevelProgressManager>()
                .FromInstance(levelProgressManager)
                .AsSingle();
        }

        private static GameObject CreateParent(string name)
        {
            var debug = new GameObject {name = name};
            DontDestroyOnLoad(debug);
            return debug;
        }

        private void InstantiateFpsCounter(GameObject parent)
        {
            Container.InstantiatePrefab(fpsCounter)
                .transform.SetParent(parent.transform);
        }
    }
}