using DefaultNamespace;
using DefaultNamespace.Services.AdsManager;
using DefaultNamespace.Services.AdsManager.AppLovin;
using DefaultNamespace.Services.AudioManager;
using DefaultNamespace.ThemeManager;
using Firebase;
using Firebase.Analytics;
using Services.DataManipulator;
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
        [SerializeField] private GameObject lunarConsole;
        [SerializeField] private GameObject audioManager;
        [SerializeField] private GameObject adsCanvasPrefab;
        
        // services
        private DiamondCountManager diamondCountManager;
        private LevelProgressManager levelProgressManager;
        private SceneLoader sceneLoader;
        private StartBoxCountManager startBoxCountManager;
        private DiamondMultiplierLevelManager diamondMultiplierLevelManager;
        private GameObject debugParent;
        private GameObject servicesParent;

        private BannerAd bannerAd;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;
        
        
        public override void InstallBindings()
        {
            debugParent = CreateParent("Debug");
            servicesParent = CreateParent("Services");

            diamondCountManager = new DiamondCountManager();
            levelProgressManager = new LevelProgressManager();
            sceneLoader = new SceneLoader(levelProgressManager);
            startBoxCountManager = new StartBoxCountManager();
            diamondMultiplierLevelManager = new DiamondMultiplierLevelManager();
            BindVibrator();
            BindAudioManager();
            BindDiamondCountManager();
            BindDiamondMultiplierLevelManager();
            BindLevelProgressManager();
            BindSceneLoader();
            BindStartBoxCountManager();
            BindThemeManager();
            BindDiamondUI(diamondCountManager);
            //InstantiateFpsCounter(debugParent);
            BindAdsManager();

            //InstantiateLunarConsole(debugParent);
        }

        private void InstantiateLunarConsole(GameObject parent)
        {
            Container.InstantiatePrefab(lunarConsole)
                .transform.SetParent(parent.transform);
        }

        private void BindAudioManager()
        {
            var audioManager = Container.InstantiatePrefab(this.audioManager);
            var manager = audioManager.GetComponent<AudioManager>();

            Container
                .Bind<AudioManager>()
                .FromInstance(manager)
                .AsSingle();

            manager.Setup();
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

        private void BindAdsManager()
        {
            var adsManager = Container.InstantiatePrefabForComponent<AdsManager>(adsCanvasPrefab);
            Container.Bind<AdsManager>().FromInstance(adsManager).AsSingle();
            Container.Bind<BannerAd>().FromInstance(adsManager.GetComponent<BannerAd>());
            Container.Bind<InterstitialAd>().FromInstance(adsManager.GetComponent<InterstitialAd>());
            Container.Bind<RewardedAd>().FromInstance(adsManager.GetComponent<RewardedAd>());
        }

        private void BindStartBoxCountManager()
        {
            Container
                .Bind<StartBoxCountManager>()
                .FromInstance(startBoxCountManager)
                .AsSingle();
        }
        
        private void BindDiamondMultiplierLevelManager()
        {
            Container
                .Bind<DiamondMultiplierLevelManager>()
                .FromInstance(diamondMultiplierLevelManager)
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