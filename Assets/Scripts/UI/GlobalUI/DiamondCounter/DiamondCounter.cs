using System;
using Services.DataManipulator;
using Services.DiamondCountManager;
using Services.ProgressController.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GlobalUI.DiamondCounter
{
    public class DiamondCounter : MonoBehaviour, IDataUpdatable
    {
        [SerializeField] private Text text;

        private int diamondCount => text.text != "" ? Convert.ToInt32(text.text) : 0;
        private DiamondCountManager diamondCountManager;
        private DiamondMultiplierLevelManager m_diamondMultiplierLevelManager;
        [Inject]
        private void Construct(DiamondCountManager diamondCountManager, DiamondMultiplierLevelManager _diamondMultiplierLevelManager)
        {
            this.diamondCountManager = diamondCountManager;
            diamondCountManager.AddListener(this);
            m_diamondMultiplierLevelManager = _diamondMultiplierLevelManager;
            text.text = this.diamondCountManager.GetData().ToString();
        }

        public void AddDiamond()
        {
            var count = diamondCount + m_diamondMultiplierLevelManager.GetData();
            diamondCountManager.UpdateData(count);
            text.text = Convert.ToString(count);
        }
        
        public void AddDiamond(int count)
        {
            var count2 = diamondCount + count;
            diamondCountManager.UpdateData(count2);
            text.text = Convert.ToString(count2);
        }
        
        public void MinusDiamonds(int count)
        {
            var newCount = diamondCount - count;
            diamondCountManager.UpdateData(newCount);
            text.text = Convert.ToString(newCount);
        }
        
        public void UpdateData()
        {
            text.text = Convert.ToString(diamondCountManager.GetData());
        }
    }
}