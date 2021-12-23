using DefaultNamespace.Services.AdsManager;
using UnityEngine;
using Zenject;

namespace UI.Balloon
{
    public class OpenAgainButton : MonoBehaviour
    {
        private OpenBalloonsCounter _openBalloonsCounter;
        private AdsManager adsManager;
        
        [Inject]
        private void Construct(OpenBalloonsCounter openBalloonsCounter, AdsManager _adsManager)
        {
            _openBalloonsCounter = openBalloonsCounter;
            adsManager = _adsManager;
        }
        
        public void ShowReward()
        {
            adsManager.ShowRewarded();
            
            RewardedAds.rewardedAd.OnUserEarnedReward += (_sender, _args) =>
            {
                RewardedAds.Initialize();
                _openBalloonsCounter.HideButtons();
            };
        }
    }
}