using DefaultNamespace;
using DefaultNamespace.Services.AdsManager;
using DefaultNamespace.Services.AudioManager;
using Services.DiamondCountManager;
using Services.LevelProgressManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class NextLevelButton : MonoBehaviour
    {
        [SerializeField] private bool balloon;
        
        private LevelProgressManager levelProgressManager;
        private DiamondCountManager diamondCountManager;
        private SceneLoader sceneLoader;
        private Level level;

        private AdsManager adsManager;
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(LevelProgressManager levelProgressManager, 
            DiamondCountManager diamondCountManager, SceneLoader sceneLoader,
            Level level, AudioManager _audioManager, AdsManager adsManager)
        {
            this.levelProgressManager = levelProgressManager;
            this.diamondCountManager = diamondCountManager;
            this.sceneLoader = sceneLoader;
            this.level = level;
            this.adsManager = adsManager;
            m_audioManager = _audioManager;
        }
        
        public void LoadNextLevel()
        {
            if (!balloon)
            {
                if (adsManager.InterstitialAd.IsAlready)
                {
                    MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += HiddenEvent;
                    adsManager.ShowInterstitial();
                }
                else
                {
                    GOtoNextLevel();
                }
            }
            else
            {
                GOtoNextLevel();
            }
        }

        private void HiddenEvent(string s, MaxSdkBase.AdInfo adInfo)
        {
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= HiddenEvent;
            GOtoNextLevel();
        }

        private void GOtoNextLevel()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            levelProgressManager.UpdateData(level.NextLevel);
            levelProgressManager.WriteData();
            diamondCountManager.WriteData();

            if (balloon)
                sceneLoader.SetNextScene("Balloon");
            else
                sceneLoader.SetNextScene(level.NextLevel);

            SceneManager.LoadScene("Loader");
        }
    }
}