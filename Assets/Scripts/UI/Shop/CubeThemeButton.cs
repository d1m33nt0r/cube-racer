using DefaultNamespace.Services.ShopData;
using DefaultNamespace.ThemeManager;
using UnityEngine;

namespace UI.Shop
{
    public class CubeThemeButton : MonoBehaviour
    {
        [SerializeField] private GameObject CUBE_DEMO;
        
        [SerializeField] private GameObject cubeDemo;
        [SerializeField] private bool first;

        private ThemeManager themeManager;
        public BoxTheme boxTheme;
        public BoxTheme BoxTheme => boxTheme;

        private CubeButtonsConstructor cubeButtonsConstructor;
        
        public void Bind(CubeButtonsConstructor cubeButtonsConstructor)
        {
            this.cubeButtonsConstructor = cubeButtonsConstructor;
        }
        
        public void Construct(BoxTheme boxTheme, ThemeManager themeManager)
        {
            this.boxTheme = boxTheme;
            this.themeManager = themeManager;
            
            if (boxTheme.bought)
                ActiveDemoCube();
        }

        public void ActiveDemoCube()
        {
            if (transform.childCount > 0) Destroy(transform.GetChild(0).gameObject);
            CUBE_DEMO.GetComponent<Renderer>().material = themeManager.GetTheme();
            cubeDemo.SetActive(true);
        }

        public void SelectTheme()
        {
            if (boxTheme.bought)
            {
                themeManager.SetCurrentTheme(boxTheme.key);
                cubeButtonsConstructor.SetCurrentTheme();   
            }
        }
    }
}