using GoogleMobileAds.Api;
using UnityEngine;

namespace DefaultNamespace.Services.AdsManager
{
    public static class RewardedAds
    {
        public static RewardedAd rewardedAd;
        private static string bannerUnitId = "ca-app-pub-3940256099942544/6300978111";
        public static bool IsLoaded => rewardedAd.IsLoaded();
        
        public static void Initialize()
        {
            rewardedAd = new RewardedAd(bannerUnitId);
            
            AdRequest request = new AdRequest.Builder().Build();

            rewardedAd.LoadAd(request);
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