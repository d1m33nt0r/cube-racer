using DefaultNamespace.Services.AdsManager;
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
        
        [Inject]
        private void Construct(AdsManager _adsManager, DiamondMultiplierLevelManager _diamondMultiplierLevelManager)
        {
            m_adsManager = _adsManager;
            m_diamondMultiplierLevelManager = _diamondMultiplierLevelManager;
            _text.text = _diamondMultiplierLevelManager.GetData().ToString();
        }

        public void ShowReward()
        {
            var parsed = int.TryParse(_text.text, out var level);
        
            if (!parsed || level > 3) return;

            m_adsManager.ShowRewarded();

            RewardedAds.rewardedAd.OnUserEarnedReward += (_sender, _reward) =>
            {
                _text.text = (level + 1).ToString();
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