using GoogleMobileAds.Api;
using UnityEngine;

namespace DefaultNamespace.Services.AdsManager
{
    public static class RewardedAds
    {
        public static RewardedAd rewardedAd;
        private static string bannerUnitId = "ca-app-pub-3940256099942544/5224354917";
        public static bool IsLoaded => rewardedAd.IsLoaded();
        
        public static void Initialize()
        {
            MobileAds.Initialize(initStatus => { });
            rewardedAd = new RewardedAd(bannerUnitId);
            
            AdRequest request = new AdRequest.Builder().Build();

            rewardedAd.LoadAd(request);
            rewardedAd.OnAdLoaded += (sender, args) =>
            {
                Debug.Log("Rewarded Ads loaded");
            };
            rewardedAd.OnAdFailedToLoad += (_sender, _args) =>
            {
                Debug.Log("Rewarded Ads failed to load");
            };
            rewardedAd.OnAdOpening += (_sender, _args) =>
            {
                Debug.Log("Rewarded Ads opening");
            };
            rewardedAd.OnAdFailedToShow += (_sender, _args) =>
            {
                Debug.Log("Rewarded Ads failed to show");
            };
        }

        public static void Show()
        {
            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
                rewardedAd.OnAdClosed += (_sender, _args) =>
                {
                    //Time.timeScale = 1;
                    Initialize();
                };
            }
        }
    }
}