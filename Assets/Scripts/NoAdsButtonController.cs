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
        [SerializeField] private Button _button;

        private void Start()
        {
            if (!PlayerPrefs.HasKey(removeAds))
                PlayerPrefs.SetInt(removeAds, 0);   
            
            var adsPurchased = PlayerPrefs.GetInt(removeAds);
            
            if (adsPurchased == 1)
            {
                background.enabled = false;
                _button.enabled = false;
                text.enabled = false;
                palka.enabled = false;
            }
        }

        public void DisableButton()
        {
            background.enabled = false;
            _button.enabled = false;
            text.enabled = false;
            palka.enabled = false;
        }
    }
}