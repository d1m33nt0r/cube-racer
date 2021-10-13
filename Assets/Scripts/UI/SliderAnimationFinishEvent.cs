using UnityEngine;
using Zenject;

namespace UI
{
    public class SliderAnimationFinishEvent : MonoBehaviour
    {
        private SceneLoader sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        public void LoadNextScene()
        {
            sceneLoader.LoadNextScene();
        }
    }
}