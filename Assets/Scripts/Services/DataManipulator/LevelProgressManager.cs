using Services.ProgressController.Interfaces;
using UnityEngine;

namespace Services.LevelProgressManager
{
    public class LevelProgressManager : IDataManipulator
    {
        private string currentLevel = "";
        private string key = "current_level";
        private string keyLevelNumber = "current_level_number";
        public int currentLevelNumber = 1;
        
        public LevelProgressManager()
        {
            if (!PlayerPrefs.HasKey(keyLevelNumber))
                PlayerPrefs.SetInt(keyLevelNumber, 1);
            
            if (!PlayerPrefs.HasKey(key))
                PlayerPrefs.SetString(key, "Level_0");
            
            ReadData();
        }

        public void ReadData()
        {
            currentLevel = PlayerPrefs.GetString(key);
            currentLevelNumber = PlayerPrefs.GetInt(keyLevelNumber);
        }

        public void UpdateData(string currentLevel)
        {
            this.currentLevel = currentLevel;
            currentLevelNumber++;
        }
        
        public void WriteData()
        {
            PlayerPrefs.SetString(key, currentLevel);
            PlayerPrefs.SetInt(keyLevelNumber, currentLevelNumber);
        }

        public string GetCurrentLevelString()
        {
            return currentLevel;
        }
    }
}