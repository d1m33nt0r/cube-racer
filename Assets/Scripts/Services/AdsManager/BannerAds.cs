using System;
using GoogleMobileAds.Api;

namespace DefaultNamespace.Services.AdsManager
{
    public static class BannerAds
    {
        private static BannerView bannerView;

        private static string bannerUnitId = "ca-app-pub-3940256099942544/6300978111";

        public static void Initialize()
        {
            MobileAds.Initialize(initStatus => { });
            RequestBanner();
            bannerView.OnAdLoaded += Show;
        }
        
        private static void RequestBanner()
        {
            // Create a 320x50 banner at the top of the screen.
            bannerView = new BannerView(bannerUnitId, AdSize.Banner, AdPosition.Bottom);
            
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the banner with the request.
            bannerView.LoadAd(request);
        }

        public static void Show(object sender, EventArgs args)
        {
            bannerView.Show();
        }

        public static void Hide()
        {
            bannerView.Hide();
        }
    }
}