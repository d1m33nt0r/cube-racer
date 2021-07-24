using Services.ProgressController;
using Services.ProgressController.Interfaces;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GeneralInstaller : MonoInstaller
    {
        [SerializeField] private GameObject fpsCounter;

        private DiamondCountManager diamondCountManager;
        private GameObject debug;
        
        public override void InstallBindings()
        {
            debug = CreateDebugParent();
            diamondCountManager = new DiamondCountManager();
            
            BindDiamondCountManager();
            InitializeFpsCounter(debug);
        }

        private void BindDiamondCountManager()
        {
            Container
                .Bind<DiamondCountManager>()
                .FromInstance(diamondCountManager)
                .AsSingle();
        }

        private static GameObject CreateDebugParent()
        {
            var debug = new GameObject {name = "Debug"};
            DontDestroyOnLoad(debug);
            return debug;
        }

        private void InitializeFpsCounter(GameObject debug)
        {
            Container.InstantiatePrefab(fpsCounter)
                .transform.SetParent(debug.transform);
        }
    }
}