using DefaultNamespace.Services.AdsManager;
using DefaultNamespace.Services.AudioManager;
using Services.DataManipulator;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Shop
{
    public class DiamondMultiplierButton : MonoBehaviour
    {
        [SerializeField] private Text _text;
    
        private AdsManager m_adsManager;
        private DiamondMultiplierLevelManager m_diamondMultiplierLevelManager;
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(AdsManager _adsManager, DiamondMultiplierLevelManager _diamondMultiplierLevelManager, AudioManager _audioManager)
        {
            m_adsManager = _adsManager;
            m_diamondMultiplierLevelManager = _diamondMultiplierLevelManager;
            _text.text = _diamondMultiplierLevelManager.GetData().ToString();
            m_audioManager = _audioManager;
        }

        public void ShowReward()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            var parsed = int.TryParse(_text.text, out var level);
        
            if (!parsed || level > 3) return;

            m_adsManager.ShowRewarded();

            RewardedAds.rewardedAd.OnUserEarnedReward += (_sender, _reward) =>
            {
                _text.text = (level + 1).ToString();
                BannerAds.Show();
                UpgradeStartBoxCount();
            };
        }
        
        private void UpgradeStartBoxCount()
        {
            var parsed = int.TryParse(_text.text, out var level);
            if (!parsed) return;
            
            m_diamondMultiplierLevelManager.UpdateData(level);
            m_diamondMultiplierLevelManager.WriteData();
                
        }
    }
}