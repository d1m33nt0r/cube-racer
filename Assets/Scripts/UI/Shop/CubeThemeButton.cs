using System;
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
        
        private CubeButtonsConstructor cubeButtonsConstructor;
        private ThemeManager themeManager;
        
        public BoxTheme boxTheme;
        public BoxTheme BoxTheme => boxTheme;
        
        public void Bind(CubeButtonsConstructor cubeButtonsConstructor)
        {
            this.cubeButtonsConstructor = cubeButtonsConstructor;
        }

        private void Start()
        {
            //var camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            //var screenPoint = camera.ViewportToScreenPoint(transform.position);
            //Vector3 cubePosition;
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, transform.position, camera,
            //   out cubePosition);
            //cubePosition.z = cubePosition.z - 0.1f;
            //cubeDemo.transform.position = cubePosition;
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
            CUBE_DEMO.GetComponent<MeshFilter>().sharedMesh = themeManager.GetTheme();
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