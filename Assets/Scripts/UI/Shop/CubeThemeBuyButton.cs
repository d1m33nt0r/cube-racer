using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Services.ShopData;
using DefaultNamespace.ThemeManager;
using UnityEngine;
using Zenject;

namespace UI.Shop
{
    public class CubeThemeBuyButton : MonoBehaviour
    {
        private List<CubeThemeButton> buttons => transform.parent.GetComponent<CubeButtonsConstructor>().ThemeButtons;

        private ThemeManager themeManager;
        
        [Inject]
        private void Construct(ThemeManager themeManager)
        {
            this.themeManager = themeManager;
        }
        
        public void ByRandomCube()
        {
            var themeButtons = buttons.Where(button => !button.boxTheme.bought).ToList();
            
            var randNum = Random.Range(0, themeButtons.Count - 1);
            
            themeManager.BuyCubeTheme(themeButtons[randNum].boxTheme.key);
            themeButtons[randNum].boxTheme.bought = true;
            themeButtons[randNum].ActiveDemoCube();
        }
    }
}