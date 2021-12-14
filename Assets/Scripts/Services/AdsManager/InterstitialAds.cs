using System;
using GoogleMobileAds.Api;

namespace DefaultNamespace.Services.AdsManager
{
    public static class InterstitialAds
    {
        public static InterstitialAd InterstitialAd => interstitialAd;
        
        private static InterstitialAd interstitialAd;
        
        private static string interstitialUnitId = "ca-app-pub-3940256099942544/1033173712";

        public static void Initialize()
        {
            MobileAds.Initialize(initStatus => { });
            LoadAds();
        }
        
        private static void LoadAds()
        {
            interstitialAd = new InterstitialAd(interstitialUnitId);
            AdRequest adRequest = new AdRequest.Builder().Build();
            interstitialAd.LoadAd(adRequest);
        }
        
        public static void Show()
        {
            if (interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
                interstitialAd.OnAdClosed += (_sender, _args) => LoadAds();
            }
        }
    }
}