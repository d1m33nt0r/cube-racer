using DefaultNamespace.Services.AdsManager;
using GoogleMobileAds.Api;
using Services.DiamondCountManager;
using UnityEngine;
using Zenject;

namespace UI.Shop
{
    public class AdsButton : MonoBehaviour
    {
        private AdsManager m_adsManager;
        private DiamondCountManager m_diamondCountManager;
            
        [Inject]
        private void Construct(AdsManager _adsManager, DiamondCountManager _diamondCountManager)
        {
            m_adsManager = _adsManager;
            m_diamondCountManager = _diamondCountManager;
        }
        
        public void Get1000Diamonds()
        {
            m_adsManager.ShowRewarded();
            RewardedAds.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        }
        
        public void HandleUserEarnedReward(object sender, Reward args)
        {
            m_diamondCountManager.AddDiamondsAndSave(1000);
        }
    }
}