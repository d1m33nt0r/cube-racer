using DefaultNamespace.Services.ShopData;
using DefaultNamespace.ThemeManager;
using UnityEngine;
using Zenject;

namespace UI.Shop
{
    public class CharacterThemeButton : MonoBehaviour
    {
        [SerializeField] private GameObject CHARACTER_DEMO;
        [SerializeField] private GameObject characterDemo;

        private CharacterButtonsConstructor characterButtonsConstructor;
        private ThemeManager themeManager;
        
        public CharacterTheme characterTheme;
        public CharacterTheme CharacterTheme => characterTheme;
        
        public void Bind(CharacterButtonsConstructor characterButtonsConstructor)
        {
            this.characterButtonsConstructor = characterButtonsConstructor;
        }

        public void Construct(CharacterTheme characterTheme, ThemeManager themeManager)
        {
            this.characterTheme = characterTheme;
            this.themeManager = themeManager;
            
            if (characterTheme.bought)
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
            if (characterTheme.bought)
            {
                themeManager.SetCurrentBoxTheme(characterTheme.key);
                characterButtonsConstructor.SetCurrentTheme();   
            }
        }
    }
}