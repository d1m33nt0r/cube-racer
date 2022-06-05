using UnityEngine;

namespace DefaultNamespace.Services.AdsManager.AppLovin
{
    public class BannerAd : MonoBehaviour
    {
        private string bannerAdUnitId = "dcbf366c59421dd5"; // Retrieve the ID from your account

        public void InitializeBannerAds()
        {
            // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
            // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
            MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

            // Set background or background color for banners to be fully functional
            var backgroundColor = Color.white;
            MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, backgroundColor);

            MaxSdkCallbacks.Banner.OnAdLoadedEvent      += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent  += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent     += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent    += OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent   += OnBannerAdCollapsedEvent;
        }

        public void ShowBanner()
        {
            MaxSdk.ShowBanner(bannerAdUnitId);
        }

        public void HideBanner()
        {
            MaxSdk.HideBanner(bannerAdUnitId);
        }
        
        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

        private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {}

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

        private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)  {}

        private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}
    }
}