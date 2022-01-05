using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class SliderAnimationFinishEvent : MonoBehaviour
    {
        [SerializeField] private Slider loadingSlider;
        
        private SceneLoader sceneLoader;
        private AsyncOperation loadingOperation;
        
        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        private void Start()
        {
            LoadNextScene();
        }

        private void LoadNextScene()
        {
            loadingOperation = sceneLoader.LoadNextScene();
            StartCoroutine(LoadingAnimation());
        }

        private IEnumerator LoadingAnimation()
        {
            while (Math.Abs(loadingOperation.progress - 1) > 0.001f)
            {
                loadingSlider.value = loadingOperation.progress;
                yield return null;
            }
        }
    }
}