using System;
using DefaultNamespace.Services.AdsManager;
using DefaultNamespace.Services.AudioManager;
using Services.DataManipulator;
using Services.DiamondCountManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace UI.Shop
{
    public class DiamondMultiplierButton : MonoBehaviour
    {
        [SerializeField] private Text levelText;
        [SerializeField] private int price;
        [SerializeField] private Image diamondPriceImage;
        [SerializeField] private Text diamondTextPriceImage;
        [SerializeField] private Text freeAdsText;
        [SerializeField] private Image adsImage;
        
        private AdsManager m_adsManager;
        private DiamondMultiplierLevelManager m_diamondMultiplierLevelManager;
        private AudioManager m_audioManager;
        private int isAds;
        private DiamondCountManager diamondCountManager;
        
        [Inject]
        private void Construct(AdsManager _adsManager, DiamondMultiplierLevelManager _diamondMultiplierLevelManager, AudioManager _audioManager, DiamondCountManager diamondCountManager)
        {
            m_adsManager = _adsManager;
            m_diamondMultiplierLevelManager = _diamondMultiplierLevelManager;
            levelText.text = _diamondMultiplierLevelManager.GetData().ToString();
            m_audioManager = _audioManager;
            this.diamondCountManager = diamondCountManager;
        }
        
        private void Start()
        {
        
            isAds = Random.Range(0, 3);
            if (isAds == 1)
            {
                diamondPriceImage.enabled = false;
                diamondTextPriceImage.enabled = false;
                freeAdsText.enabled = true;
                adsImage.enabled = true;
            }
            else
            {
                diamondPriceImage.enabled = true;
                diamondTextPriceImage.enabled = true;
                freeAdsText.enabled = false;
                adsImage.enabled = false;
            }
        } 

        public void ShowReward()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            
            if (isAds == 1 && m_diamondMultiplierLevelManager.GetData() < 4)
            {
                if (m_adsManager.RewardedAd.IsRewardedAdAlready)
                { 
                    m_adsManager.ShowRewarded();
                    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
                    MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (s, info) => MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEvent;
                }
            }
            else if(isAds == 0 || isAds == 2 && m_diamondMultiplierLevelManager.GetData() < 4)
            {
                if (price <= diamondCountManager.GetDiamondCount() && m_diamondMultiplierLevelManager.GetData() < 4)
                {
                    diamondCountManager.UpdateData(diamondCountManager.GetDiamondCount() - price);
                    m_diamondMultiplierLevelManager.UpdateData(m_diamondMultiplierLevelManager.GetData() + 1);
                    diamondCountManager.WriteData();
                    m_diamondMultiplierLevelManager.WriteData();

                    levelText.text = Convert.ToString(m_diamondMultiplierLevelManager.GetData());
                }
            }
            
            
            /*
            var parsed = int.TryParse(levelText.text, out var level);
        
            if (!parsed || level > 3) return;

            m_adsManager.ShowRewarded();
            levelText.text = (level + 1).ToString();
            UpgradeStartBoxCount();*/
        }
        
        private void OnAdReceivedRewardEvent(string s, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            m_diamondMultiplierLevelManager.UpdateData(m_diamondMultiplierLevelManager.GetData() + 1);
            m_diamondMultiplierLevelManager.WriteData();
            GetComponent<Button>().interactable = false;
            levelText.text = Convert.ToString(m_diamondMultiplierLevelManager.GetData());
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEvent;
        }
        
        private void UpgradeStartBoxCount()
        {
            var parsed = int.TryParse(levelText.text, out var level);
            if (!parsed) return;
            
            m_diamondMultiplierLevelManager.UpdateData(level);
            m_diamondMultiplierLevelManager.WriteData();
                
        }
    }
}