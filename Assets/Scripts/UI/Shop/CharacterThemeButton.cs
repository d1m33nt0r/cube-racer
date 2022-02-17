using DefaultNamespace.Services.AudioManager;
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

        private AudioManager m_audioManager;
        
        public CharacterTheme characterTheme;
        public CharacterTheme CharacterTheme => characterTheme;
        
        public void Bind(CharacterButtonsConstructor characterButtonsConstructor)
        {
            this.characterButtonsConstructor = characterButtonsConstructor;
        }

        [Inject]
        private void Inject(AudioManager _audioManager)
        {
            m_audioManager = _audioManager;
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
            var skin = Instantiate(themeManager.GetCurrentCharacterTheme(), CHARACTER_DEMO.transform.position, Quaternion.identity, CHARACTER_DEMO.transform);
            skin.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            characterDemo.SetActive(true);
        }

        public void SelectTheme()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            if (characterTheme.bought)
            {
                themeManager.SetCurrentBoxTheme(characterTheme.key);
                characterButtonsConstructor.SetCurrentTheme();   
            }
        }
    }
}