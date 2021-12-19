using DefaultNamespace.Services.AdsManager;
using UnityEngine;
using Zenject;

namespace UI.Balloon
{
    public class OpenAgainButton : MonoBehaviour
    {
        private OpenBalloonsCounter _openBalloonsCounter;
        
        [Inject]
        private void Construct(OpenBalloonsCounter openBalloonsCounter)
        {
            _openBalloonsCounter = openBalloonsCounter;
        }
        
        public void ShowReward()
        {
            RewardedAds.Show();
            
            RewardedAds.rewardedAd.OnAdClosed += (_sender, _args) =>
            {
                RewardedAds.Initialize();
                _openBalloonsCounter.HideButtons();
            };
        }
    }
}