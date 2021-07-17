using UnityEngine;
using Zenject;

namespace Installers
{
    public class GeneralInstaller : MonoInstaller
    {
        [SerializeField] private GameObject fpsCounter;

        public override void InstallBindings()
        {
            var debug = CreateDebugParent();

            InitializeFpsCounter(debug);
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