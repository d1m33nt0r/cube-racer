using System;
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
        
        [Inject]
        private void Construct(DiamondCountManager diamondCountManager)
        {
            this.diamondCountManager = diamondCountManager;
            diamondCountManager.AddListener(this);
            text.text = this.diamondCountManager.GetData().ToString();
        }

        public void AddDiamond()
        {
            var count = diamondCount + 1;
            diamondCountManager.UpdateData(count);
            text.text = Convert.ToString(count);
        }

        
        
        public void MinusDiamonds(int count)
        {
            var newCount = diamondCount - count;
            diamondCountManager.UpdateData(newCount);
            text.text = Convert.ToString(newCount);
        }
        
        public void WriteData()
        {
            diamondCountManager.WriteData();
        }

        public void UpdateData()
        {
            text.text = Convert.ToString(diamondCountManager.GetData());
        }
    }
}