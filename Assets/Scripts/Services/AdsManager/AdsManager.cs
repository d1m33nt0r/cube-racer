using DefaultNamespace.Services.AdsManager.AppLovin;
using UnityEngine;
using UnityEngine.SceneManagement;
using InterstitialAd = DefaultNamespace.Services.AdsManager.AppLovin.InterstitialAd;
using RewardedAd = DefaultNamespace.Services.AdsManager.AppLovin.RewardedAd;

namespace DefaultNamespace.Services.AdsManager
{

    public enum AdsMediation
    {
        AppLovin, 
        AdMob
    }
    
    public class AdsManager : MonoBehaviour
    {
        [SerializeField] private AdsMediation adsMediation;

        private const string removeAds = "removeAds";
        public bool adsIsDisabled = false;
        
        public InterstitialAd InterstitialAd => interstitialAd;
        public BannerAd BannerAd => bannerAd;
        public RewardedAd RewardedAd => rewardedAd;
        
        [SerializeField] private InterstitialAd interstitialAd;
        [SerializeField] private BannerAd bannerAd;
        [SerializeField] private RewardedAd rewardedAd;
        
        private void Start()
        {
            if (PlayerPrefs.HasKey(removeAds))
            {
                if (PlayerPrefs.GetInt(removeAds) == 1)
                {
                    adsIsDisabled = true;
                }
                else
                {
                    adsIsDisabled = false;
                }
            }
            else
            {
                adsIsDisabled = false;
            }
            
            switch (adsMediation)
            {
                case AdsMediation.AppLovin:
                    MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration => 
                    {
                        bannerAd.InitializeBannerAds();
                        interstitialAd.InitializeInterstitialAds();
                        rewardedAd.InitializeRewardedAds();
                        
                        if (!adsIsDisabled) ShowBanner();
                    };
                    
                    MaxSdk.SetSdkKey("8CzO4IBcwXI7GDNAT_Nwk6Le3ED5bvZMBWDTVtdyiH10RVwoHUsv4TTH8LKGf_VXMGKXlZ7JzPcdkqtoEARAQR");
                    MaxSdk.InitializeSdk();
                    break;
                case AdsMediation.AdMob:
                    BannerAds.Initialize();
                    InterstitialAds.Initialize();
                    RewardedAds.Initialize();
                    break;
            }
            
            //SceneManager.sceneLoaded += Handle;
        }

        public void DisableAds()
        {
            bannerAd.HideBanner();
            adsIsDisabled = true;
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
            switch (adsMediation)
            {
                case AdsMediation.AppLovin:
                    bannerAd.ShowBanner();
                    break;
                case AdsMediation.AdMob:
                    BannerAds.Show();
                    break;
            }
        }

        private void HideBanner()
        {
            switch (adsMediation)
            {
                case AdsMediation.AppLovin:
                    bannerAd.HideBanner();
                    break;
                case AdsMediation.AdMob:
                    BannerAds.Hide();
                    break;
            }
        }

        public void ShowRewarded()
        {
            switch (adsMediation)
            {
                case AdsMediation.AppLovin:
                    if (rewardedAd.IsRewardedAdAlready)
                        rewardedAd.Show();
                    break;
                case AdsMediation.AdMob:
                    RewardedAds.Show();
                    break;
            }
        }
        
        public void ShowInterstitial()
        {
            switch (adsMediation)
            {
                case AdsMediation.AppLovin:
                    interstitialAd.Show();
                    break;
                case AdsMediation.AdMob:
                    InterstitialAds.Show();
                    break;
            }
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