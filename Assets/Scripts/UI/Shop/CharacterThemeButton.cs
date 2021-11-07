using DefaultNamespace.Services.ShopData;
using DefaultNamespace.ThemeManager;
using UnityEngine;

namespace UI.Shop
{
    public class CharacterThemeButton : MonoBehaviour
    {
        [SerializeField] private GameObject CHARACTER_DEMO;
        [SerializeField] private GameObject characterDemo;

        private CharacterButtonsConstructor characterButtonsConstructor;
        private ThemeManager themeManager;
        
        public CharacterTheme boxTheme;
        public CharacterTheme BoxTheme => boxTheme;
        
        public void Bind(CharacterButtonsConstructor characterButtonsConstructor)
        {
            this.characterButtonsConstructor = characterButtonsConstructor;
        }

        public void Construct(CharacterTheme boxTheme, ThemeManager themeManager)
        {
            this.boxTheme = boxTheme;
            this.themeManager = themeManager;
            
            if (boxTheme.bought)
                ActiveDemoCharacter();
        }

        public void ActiveDemoCharacter()
        {
            if (transform.childCount > 0) Destroy(transform.GetChild(0).gameObject);
            CHARACTER_DEMO.GetComponent<MeshFilter>().sharedMesh = themeManager.GetCurrentCharacterTheme();
            characterDemo.SetActive(true);
        }

        public void SelectTheme()
        {
            if (boxTheme.bought)
            {
                themeManager.SetCurrentBoxTheme(boxTheme.key);
                characterButtonsConstructor.SetCurrentTheme();   
            }
        }
    }
}