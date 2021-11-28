using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Services.ShopData;
using UnityEngine;

namespace DefaultNamespace.ThemeManager
{
    public class ThemeManager : MonoBehaviour
    {
        private BoxTheme currentBoxTheme;
        private CharacterTheme currentCharacterTheme;
        
        [SerializeField] private List<BoxTheme> boxThemes;
        [SerializeField] private List<CharacterTheme> characterThemes;

        public CharacterTheme CurrentCharacterTheme => currentCharacterTheme;
        public BoxTheme CurrentBoxTheme => currentBoxTheme;
        
        public List<BoxTheme> BoxThemes => boxThemes;
        public List<CharacterTheme> CharacterThemes => characterThemes;
        
        private void Awake()
        {
            InitializeBoxThemes();
            InitializeCharacterThemes();
        }

        private void InitializeCharacterThemes()
        {
            if (!PlayerPrefs.HasKey("current_character_theme"))
            {
                PlayerPrefs.SetString("current_character_theme", "default_character_theme");
                currentCharacterTheme = characterThemes.FirstOrDefault(theme => 
                    theme.key == "default_character_theme");
            }
            else
            {
                currentCharacterTheme = characterThemes.FirstOrDefault(theme => 
                    theme.key == PlayerPrefs.GetString("current_character_theme"));
            }

            foreach (var theme in characterThemes)
            {
                if (!PlayerPrefs.HasKey(theme.key))
                    PlayerPrefs.SetInt(theme.key, Convert.ToInt32(theme.bought));
                else
                    theme.bought = Convert.ToBoolean(PlayerPrefs.GetInt(theme.key));
            }
        }

        private void InitializeBoxThemes()
        {
            if (!PlayerPrefs.HasKey("current_box_theme"))
            {
                PlayerPrefs.SetString("current_box_theme", "default_box_theme");
                currentBoxTheme = boxThemes.FirstOrDefault(theme => theme.key == "default_box_theme");
            }
            else
            {
                currentBoxTheme =
                    boxThemes.FirstOrDefault(theme => theme.key == PlayerPrefs.GetString("current_box_theme"));
            }

            foreach (var theme in boxThemes)
            {
                if (!PlayerPrefs.HasKey(theme.key))
                    PlayerPrefs.SetInt(theme.key, Convert.ToInt32(theme.bought));
                else
                    theme.bought = Convert.ToBoolean(PlayerPrefs.GetInt(theme.key));
            }
        }

        public void BuyCharacterTheme(string key)
        {
            PlayerPrefs.SetInt(key, 1);
        }

        public void SetCurrentCharacterTheme(string key)
        {
            currentBoxTheme = boxThemes.FirstOrDefault(theme => theme.key == key);
            PlayerPrefs.SetString("current_character_theme", key);
        }
        
        public Mesh GetCurrentCharacterTheme()
        {
            return characterThemes.FirstOrDefault(theme => 
                theme.key == PlayerPrefs.GetString("current_character_theme"))?.mesh;
        }
        
        public void BuyCubeTheme(string key)
        {
            PlayerPrefs.SetInt(key, 1);
        }
        
        public void SetCurrentBoxTheme(string key)
        {
            currentBoxTheme = boxThemes.FirstOrDefault(theme => theme.key == key);
            PlayerPrefs.SetString("current_box_theme", key);
        }
        
        public Material GetCurrentBoxTheme()
        {
            return boxThemes.FirstOrDefault(theme => 
                theme.key == PlayerPrefs.GetString("current_box_theme"))?.material;
        }
    }
}