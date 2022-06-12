using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class NoAdsButtonController : MonoBehaviour
    {
        private const string removeAds = "removeAds";
        
        [SerializeField] private Image background;
        [SerializeField] private Text text;
        [SerializeField] private Image palka;
        
        private void Start()
        {
            if (!PlayerPrefs.HasKey(removeAds))
                PlayerPrefs.SetInt(removeAds, 0);   
            
            var adsPurchased = PlayerPrefs.GetInt(removeAds);
            
            if (adsPurchased == 1)
            {
                background.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
                palka.gameObject.SetActive(false);
            }
        }
    }
}