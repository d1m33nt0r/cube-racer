﻿using System;
using DefaultNamespace.Services.AdsManager;
using Services.DiamondCountManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class RestartLevel : MonoBehaviour
    {
        private DiamondCountManager diamondCountManager;
        private AdsManager adsManager;
        
        [Inject]
        private void Construct(DiamondCountManager diamondCountManager, AdsManager _adsManager)
        {
            this.diamondCountManager = diamondCountManager;
            adsManager = _adsManager;
        }

        public void ReloadCurrentScene()
        {
            diamondCountManager.WriteData();
            if (InterstitialAds.InterstitialAd.IsLoaded())
            {
                adsManager.ShowInterstitial();
                InterstitialAds.InterstitialAd.OnAdClosed += HandleOnAdClosed;
            }
            else
                SceneManager.LoadScene("Loader");
        }

        private void HandleOnAdClosed(object sender, EventArgs args)
        {
            InterstitialAds.InterstitialAd.OnAdClosed -= HandleOnAdClosed;
            SceneManager.LoadScene("Loader");
        }
    }
}