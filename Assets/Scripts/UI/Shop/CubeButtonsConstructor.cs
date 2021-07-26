using System.Collections.Generic;
using DefaultNamespace.ThemeManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Shop
{
    public class CubeButtonsConstructor : MonoBehaviour
    {
        [SerializeField] private List<CubeThemeButton> themeButtons;
        [SerializeField] private Sprite selectedButton;
        [SerializeField] private Sprite unSelectedButton;
        
        
        public List<CubeThemeButton> ThemeButtons => themeButtons;
        
        private ThemeManager themeManager;

        [Inject]
        private void Construct(ThemeManager themeManager)
        {
            this.themeManager = themeManager;
        }

        private void Awake()
        {
            SetCurrentTheme();

            foreach (var themeButton in themeButtons)
            {
                themeButton.Bind(this);
            }
        }


        public void SetCurrentTheme()
        {
            var i = 0;
            
            foreach (var themeButton in themeManager.BoxThemes)
            {
                themeButtons[i].Construct(themeButton, themeManager);
                
                if (themeManager.CurrentBoxTheme.key == themeButtons[i].boxTheme.key)
                {
                    themeButtons[i].GetComponent<Image>().sprite = selectedButton;
                }
                else
                {
                    themeButtons[i].GetComponent<Image>().sprite = unSelectedButton;
                }
                
                i++;
            }
        }
    }
}