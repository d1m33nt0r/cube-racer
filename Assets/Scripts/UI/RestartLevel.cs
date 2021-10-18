using System;
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

        [Inject]
        private void Construct(DiamondCountManager diamondCountManager)
        {
            this.diamondCountManager = diamondCountManager;
        }

        public void ReloadCurrentScene()
        {
            diamondCountManager.WriteData();
            AdsManager.ShowInterstitial();
            InterstitialAds.InterstitialAd.OnAdClosed += HandleOnAdClosed;
        }

        private void HandleOnAdClosed(object sender, EventArgs args)
        {
            InterstitialAds.InterstitialAd.OnAdClosed -= HandleOnAdClosed;
            SceneManager.LoadScene("Loader");
        }
    }
}