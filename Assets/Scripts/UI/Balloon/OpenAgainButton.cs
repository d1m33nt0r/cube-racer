using DefaultNamespace.Services.AdsManager;
using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using Zenject;

namespace UI.Balloon
{
    public class OpenAgainButton : MonoBehaviour
    {
        private OpenBalloonsCounter _openBalloonsCounter;
        private AdsManager adsManager;
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(OpenBalloonsCounter openBalloonsCounter, AdsManager _adsManager, AudioManager _audioManager)
        {
            _openBalloonsCounter = openBalloonsCounter;
            adsManager = _adsManager;
            m_audioManager = _audioManager;
        }
        
        public void ShowReward()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            if (adsManager.RewardedAd.IsRewardedAdAlready)
            {
                adsManager.ShowRewarded();
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += HideButtons;
            }
        }

        private void HideButtons(string s, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            _openBalloonsCounter.HideButtons();
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= HideButtons;
        }
    }
}