using System;
using DefaultNamespace.Services.AdsManager;
using DefaultNamespace.Services.AudioManager;
using Services.DiamondCountManager;
using Services.StartBoxCountManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class StartBoxCountBuyButton : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private Text level;

    [SerializeField] private Image diamondPriceImage;
    [SerializeField] private Text diamondTextPriceImage;
    [SerializeField] private Text freeAdsText;
    [SerializeField] private Image adsImage;
    
    private AdsManager adsManager;
    private StartBoxCountManager startBoxCountManager;
    private BoxController boxController;
    private DiamondCountManager diamondCountManager;
    private AudioManager m_audioManager;

    private int isAds;

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

    [Inject]
    private void Construct(StartBoxCountManager startBoxCountManager, BoxController boxController, 
        DiamondCountManager diamondCountManager, AudioManager _audioManager, AdsManager _adsManager)
    {
        this.boxController = boxController;
        this.startBoxCountManager = startBoxCountManager;
        this.diamondCountManager = diamondCountManager;
        m_audioManager = _audioManager;
        adsManager = _adsManager;
        //PlayerPrefs.SetInt("start_box_count", 1);
        level.text = Convert.ToString(startBoxCountManager.GetData());
    }
    
    public void UpgradeStartBoxCount()
    {
        m_audioManager.uiAudioSource.PlayButtonClickSound();

        if (isAds == 1 && startBoxCountManager.GetData() < 4)
        {
            if (adsManager.RewardedAd.IsRewardedAdAlready)
            { 
                adsManager.ShowRewarded();
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
                MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (s, info) => MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEvent;
            }
        }
        else if(isAds == 0 || isAds == 2 && startBoxCountManager.GetData() < 4)
        {
            if (price <= diamondCountManager.GetDiamondCount() && startBoxCountManager.GetData() < 4)
            {
                diamondCountManager.UpdateData(diamondCountManager.GetDiamondCount() - price);
                startBoxCountManager.UpdateData(startBoxCountManager.GetData() + 1);
                diamondCountManager.WriteData();
                startBoxCountManager.WriteData();
                boxController.InstantiateNewBox();
            
                level.text = Convert.ToString(startBoxCountManager.GetData());
            }
        }
    }
    
    private void OnAdReceivedRewardEvent(string s, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        startBoxCountManager.UpdateData(startBoxCountManager.GetData() + 1);
        startBoxCountManager.WriteData();
        boxController.InstantiateNewBox();
        GetComponent<Button>().interactable = false;
        level.text = Convert.ToString(startBoxCountManager.GetData());
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEvent;
    }
}