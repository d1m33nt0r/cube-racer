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
            ShowBanner();
            SceneManager.sceneLoaded += Handle;
        }

        private void Handle(Scene _arg0, LoadSceneMode _loadSceneMode)
        {
            /*switch (_arg0.name)
            {
                case "Loader":
                    EnableCanvas();
                    break;
                case "Shop":
                    EnableCanvas();
                    break;
                case "Balloon":
                    EnableCanvas();
                    break;
                default:
                    EnableCanvas();
                    break;
            }*/
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
            RewardedAds.Show();
        }
        
        public void ShowInterstitial()
        {
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