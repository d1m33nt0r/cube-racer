using DefaultNamespace;
using DefaultNamespace.Services.AdsManager;
using DefaultNamespace.ThemeManager;
using Services.DiamondCountManager;
using Services.LevelProgressManager;
using Services.StartBoxCountManager;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GeneralInstaller : MonoInstaller
    {
        [SerializeField] private GameObject fpsCounter;
        [SerializeField] private DiamondUI diamondUI;
        [SerializeField] private ThemeManager themeManager;
        [SerializeField] private Vibrator vibrator;

        // services
        private DiamondCountManager diamondCountManager;
        private LevelProgressManager levelProgressManager;
        private SceneLoader sceneLoader;
        private StartBoxCountManager startBoxCountManager;

        private GameObject debugParent;
        private GameObject servicesParent;

        public override void InstallBindings()
        {
            debugParent = CreateParent("Debug");
            servicesParent = CreateParent("Services");

            diamondCountManager = new DiamondCountManager();
            levelProgressManager = new LevelProgressManager();
            sceneLoader = new SceneLoader(levelProgressManager);
            startBoxCountManager = new StartBoxCountManager();

            BindDiamondCountManager();
            BindLevelProgressManager();
            BindSceneLoader();
            BindStartBoxCountManager();
            BindThemeManager();
            BindDiamondUI(diamondCountManager);
            InstantiateFpsCounter(debugParent);
            InitializeAds();
            BindVibrator();
        }

        private void BindVibrator()
        {
            var vibrator = Container.InstantiatePrefab(this.vibrator);

            Container
                .Bind<Vibrator>()
                .FromInstance(vibrator.GetComponent<Vibrator>())
                .AsSingle();
        }

        private void BindThemeManager()
        {
            var themeManager = Container.InstantiatePrefab(this.themeManager);

            Container
                .Bind<ThemeManager>()
                .FromInstance(themeManager.GetComponent<ThemeManager>())
                .AsSingle();
        }

        private void InitializeAds()
        {
            InterstitialAds.Initialize();
                    InterstitialAds.LoadAds();
        }

        private void BindStartBoxCountManager()
        {
            Container
                .Bind<StartBoxCountManager>()
                .FromInstance(startBoxCountManager)
                .AsSingle();
        }

        private void BindSceneLoader()
        {
            Container
                .Bind<SceneLoader>()
                .FromInstance(sceneLoader)
                .AsSingle();
        }

        private void BindDiamondUI(DiamondCountManager diamondCountManager)
        {
            var diamondUI = Container.InstantiatePrefab(this.diamondUI);
            diamondUI.GetComponent<DiamondUI>().Construct(diamondCountManager);

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