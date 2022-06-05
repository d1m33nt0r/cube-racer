using System;
using DefaultNamespace.Services.AdsManager;
using DefaultNamespace.Services.AudioManager;
using Services.DiamondCountManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class RestartLevel : MonoBehaviour
    {
        private DiamondCountManager diamondCountManager;
        private AdsManager adsManager;
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(DiamondCountManager diamondCountManager, AdsManager _adsManager, AudioManager _audioManager)
        {
            this.diamondCountManager = diamondCountManager;
            adsManager = _adsManager;
            m_audioManager = _audioManager;
        }

        public void ReloadCurrentScene()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            diamondCountManager.WriteData();
            if (adsManager.InterstitialAd.IsAlready)
            {
                adsManager.ShowInterstitial();
                MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += HandleOnAdClosed;
            }
            else
                SceneManager.LoadScene("Loader");
        }

        private void HandleOnAdClosed(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= HandleOnAdClosed;
            SceneManager.LoadScene("Loader");
        }
    }
}