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
            var multiplier = diamondMultiplier.GetMultiplier();
            var sessionDiamondCount = sessionDiamondCounter.GetCount();
            var text = "";
            if (multiplier > 1)
            {
                text = Convert.ToString(multiplier * sessionDiamondCount - sessionDiamondCount);
            }
            else
            {
                text = Convert.ToString(multiplier * sessionDiamondCount);
            }
            text = Convert.ToString(Int32.Parse(text) + sessionDiamondCount);
            diamondCount.text = text;
            diamondCountManager.UpdateData(Convert.ToInt32(diamondCount.text) + diamondCountManager.GetDiamondCount());
        }
    }
}