using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Services.AdsManager
{
    public class AdsManager : MonoBehaviour
    {
        private void Awake()
        {
            InterstitialAds.Initialize();
            InterstitialAds.LoadAds();
            BannerAds.Initialize();
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += ShowBanner;
        }

        private static void ShowBanner(Scene scene, LoadSceneMode mode)
        {
            BannerAds.Show(null, EventArgs.Empty);
        }

        private static void HideBanner()
        {
            BannerAds.Hide();
        }

        public static void ShowInterstitial()
        {
            InterstitialAds.Show();
            HideBanner();
        }
    }
}