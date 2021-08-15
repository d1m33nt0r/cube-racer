using System;
using Services.DiamondCountManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class FinishMultiplier : MonoBehaviour
    {
        [SerializeField] private Text multiplierText;
        [SerializeField] private Text diamondCount;
        
        private DiamondMultiplier diamondMultiplier;
        private SessionDiamondCounter sessionDiamondCounter;
        private DiamondCountManager diamondCountManager;

        [Inject]
        private void Construct(DiamondMultiplier diamondMultiplier, SessionDiamondCounter sessionDiamondCounter, DiamondCountManager diamondCountManager)
        {
            this.sessionDiamondCounter = sessionDiamondCounter;
            this.diamondMultiplier = diamondMultiplier;
            this.diamondCountManager = diamondCountManager;
        }
        
        public void GetCountDiamonds()
        {
            multiplierText.text = "x" + Convert.ToString(diamondMultiplier.GetMultiplier());
            diamondCount.text = Convert.ToString(diamondMultiplier.GetMultiplier() * sessionDiamondCounter.GetCount() - sessionDiamondCounter.GetCount());
            diamondCountManager.UpdateData(Convert.ToInt32(diamondCount.text) + diamondCountManager.GetData());
        }
    }
}