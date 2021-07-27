using System;
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
        
        [Inject]
        private void Construct(DiamondMultiplier diamondMultiplier, SessionDiamondCounter sessionDiamondCounter)
        {
            this.sessionDiamondCounter = sessionDiamondCounter;
            this.diamondMultiplier = diamondMultiplier;
        }
        
        public void GetCountDiamonds()
        {
            multiplierText.text = Convert.ToString(diamondMultiplier.GetMultiplier());
            diamondCount.text = Convert.ToString(diamondMultiplier.GetMultiplier() * sessionDiamondCounter.GetCount());
        }
    }
}