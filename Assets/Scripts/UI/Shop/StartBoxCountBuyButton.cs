

using System;
using DefaultNamespace.Services.AudioManager;
using Services.DiamondCountManager;
using Services.StartBoxCountManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartBoxCountBuyButton : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private Text level;

    private StartBoxCountManager startBoxCountManager;
    private BoxController boxController;
    private DiamondCountManager diamondCountManager;
    private AudioManager m_audioManager;
    
    [Inject]
    private void Construct(StartBoxCountManager startBoxCountManager, BoxController boxController, DiamondCountManager diamondCountManager, AudioManager _audioManager)
    {
        this.boxController = boxController;
        this.startBoxCountManager = startBoxCountManager;
        this.diamondCountManager = diamondCountManager;
        m_audioManager = _audioManager;
        
        //PlayerPrefs.SetInt("start_box_count", 1);
        level.text = Convert.ToString(startBoxCountManager.GetData());
    }
    
    public void UpgradeStartBoxCount()
    {
        m_audioManager.uiAudioSource.PlayButtonClickSound();
        if (price <= diamondCountManager.GetData() && startBoxCountManager.GetData() < 4)
        {
            diamondCountManager.UpdateData(diamondCountManager.GetData() - price);
            startBoxCountManager.UpdateData(startBoxCountManager.GetData() + 1);
            diamondCountManager.WriteData();
            startBoxCountManager.WriteData();
            boxController.InstantiateNewBox();
            
            level.text = Convert.ToString(startBoxCountManager.GetData());
        }
    }
}