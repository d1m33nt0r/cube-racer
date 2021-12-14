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
            BannerAds.Initialize();
            InterstitialAds.Initialize();
            RewardedAds.Initialize();

            SceneManager.sceneLoaded += Handle;
        }

        private void Handle(Scene _arg0, LoadSceneMode _loadSceneMode)
        {
            switch (_arg0.name)
            {
                case "Loader":
                    DisableCanvas();
                    break;
                case "Shop":
                    DisableCanvas();
                    break;
                case "Balloon":
                    DisableCanvas();
                    break;
                default:
                    EnableCanvas();
                    break;
            }
        }

        private void ShowBanner()
        {
            BannerAds.Show();
        }

        private void HideBanner()
        {
            BannerAds.Hide();
        }

        public void ShowRewarded()
        {
            HideBanner();
            RewardedAds.Show();
        }
        
        public void ShowInterstitial()
        {
            HideBanner();
            InterstitialAds.Show();
        }

        private void DisableCanvas()
        {
            HideBanner();
            GetComponent<Canvas>().enabled = false;
        }

        private void EnableCanvas()
        {
            GetComponent<Canvas>().enabled = true;
            ShowBanner();
        }
    }
}