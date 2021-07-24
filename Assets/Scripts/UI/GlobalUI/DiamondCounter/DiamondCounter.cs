using System;
using Services.ProgressController;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GlobalUI.DiamondCounter
{
    public class DiamondCounter : MonoBehaviour
    {
        [SerializeField] private Text text;

        private int diamondCount => Convert.ToInt32(text.text);
        private DiamondCountManager diamondCountManager;
        
        [Inject]
        private void Construct(DiamondCountManager diamondCountManager)
        {
            this.diamondCountManager = diamondCountManager;
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
    }
}