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
        
        [SerializeField] private List<BoxTheme> boxThemes;

        public BoxTheme CurrentBoxTheme => currentBoxTheme;
        
        public List<BoxTheme> BoxThemes => boxThemes;
        
        private void Awake()
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
                {
                    PlayerPrefs.SetInt(theme.key, Convert.ToInt32(theme.bought));
                }
                else
                {
                    theme.bought = Convert.ToBoolean(PlayerPrefs.GetInt(theme.key));
                }
            }
        }

        public void BuyCubeTheme(string key)
        {
            PlayerPrefs.SetInt(key, 1);
        }

        public void SetCurrentTheme(string key)
        {
            currentBoxTheme = boxThemes.FirstOrDefault(theme => theme.key == key);
            PlayerPrefs.SetString("current_box_theme", key);
        }
        
        public Mesh GetTheme()
        {
            return boxThemes.FirstOrDefault(theme => theme.key == PlayerPrefs.GetString("current_box_theme"))?.mesh;
        }
    }
}