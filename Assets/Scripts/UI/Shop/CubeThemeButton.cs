using DefaultNamespace.Services.ShopData;
using DefaultNamespace.ThemeManager;
using UnityEngine;

namespace UI.Shop
{
    public class CubeThemeButton : MonoBehaviour
    {
        [SerializeField] private GameObject CUBE_DEMO;
        [SerializeField] private GameObject cubeDemo;
        
        private CubeButtonsConstructor cubeButtonsConstructor;
        private ThemeManager themeManager;
        
        public BoxTheme boxTheme;
        public BoxTheme BoxTheme => boxTheme;
        
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
            CUBE_DEMO.GetComponent<MeshFilter>().sharedMesh = themeManager.GetCurrentBoxTheme();
            cubeDemo.SetActive(true);
        }

        public void SelectTheme()
        {
            if (boxTheme.bought)
            {
                themeManager.SetCurrentBoxTheme(boxTheme.key);
                cubeButtonsConstructor.SetCurrentTheme();   
            }
        }
    }
}