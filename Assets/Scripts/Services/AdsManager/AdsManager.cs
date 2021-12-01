using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Services.AdsManager
{
    public class AdsManager : MonoBehaviour
    {
        private void Start()
        {
            
        }

        public void BannerShow()
        {
            StartCoroutine(WaitForInitializationAndShowBannerAds());
        }
        
        private IEnumerator WaitForInitializationAndShowBannerAds()
        {
            while (!BannerAds.IsReadyToUse)
                yield return null;
            
            ShowBanner();
        }
        
        private void ShowBanner()
        {
            BannerAds.Show();
        }

        private void HideBanner()
        {
            BannerAds.Hide();
        }

        public void ShowInterstitial()
        {
            HideBanner();
            InterstitialAds.Show();
        }
    }
}