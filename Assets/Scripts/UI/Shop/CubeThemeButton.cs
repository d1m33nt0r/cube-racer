using DefaultNamespace.Services.ShopData;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class CubeThemeButton : MonoBehaviour
    {
        [SerializeField] private GameObject cubeDemo;

        [SerializeField] private bool first;
        
        public BoxTheme boxTheme;
        
        public BoxTheme BoxTheme => boxTheme;
        
        public void Construct(BoxTheme boxTheme)
        {
            this.boxTheme = boxTheme;
            if (boxTheme.bought)
                ActiveDemoCube();
        }

        public void ActiveDemoCube()
        {
            if(!first) Destroy(transform.GetChild(0).gameObject);
            
            cubeDemo.SetActive(true);
        }
    }
}