using System.Collections.Generic;
using DefaultNamespace.ThemeManager;
using UnityEngine;
using Zenject;

namespace UI.Shop
{
    public class CubeButtonsConstructor : MonoBehaviour
    {
        [SerializeField] private List<CubeThemeButton> themeButtons;

        public List<CubeThemeButton> ThemeButtons => themeButtons;
        
        private ThemeManager themeManager;

        [Inject]
        private void Construct(ThemeManager themeManager)
        {
            this.themeManager = themeManager;
        }

        private void Start()
        {
            var i = 0;
            
            foreach (var themeButton in themeManager.BoxThemes)
            {
                themeButtons[i].Construct(themeButton);
                i++;
            }
        }
    }
}