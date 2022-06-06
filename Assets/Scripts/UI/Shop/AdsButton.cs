using DefaultNamespace.Services.AdsManager;
using DefaultNamespace.Services.AudioManager;
using Services.DiamondCountManager;
using UnityEngine;
using Zenject;

namespace UI.Shop
{
    public class AdsButton : MonoBehaviour
    {
        private AdsManager m_adsManager;
        private DiamondCountManager m_diamondCountManager;
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(AdsManager _adsManager, DiamondCountManager _diamondCountManager, AudioManager _audioManager)
        {
            m_adsManager = _adsManager;
            m_diamondCountManager = _diamondCountManager;
            m_audioManager = _audioManager;
        }
        
        public void Get1000Diamonds()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            
            if (m_adsManager.RewardedAd.IsRewardedAdAlready)
            {
                m_adsManager.ShowRewarded();
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
            }
        }
        
        private void OnAdReceivedRewardEvent(string s, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            m_diamondCountManager.AddDiamondsAndSave(1000);
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEvent;
        }
    }
}