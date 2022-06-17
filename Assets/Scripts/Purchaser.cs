using DefaultNamespace.Services.AdsManager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Purchaser : MonoBehaviour
    {
        private AdsManager adsManager;

        [Inject]
        private void Construct(AdsManager adsManager)
        {
            this.adsManager = adsManager;
        }
        
        public void PurchaseDisableAds()
        {
            PlayerPrefs.SetInt("removeAds", 1);
            adsManager.DisableAds();
        }
    }
}