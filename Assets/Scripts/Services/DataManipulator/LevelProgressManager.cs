using Services.ProgressController.Interfaces;
using UnityEngine;

namespace Services.LevelProgressManager
{
    public class LevelProgressManager : IDataManipulator
    {
        private string currentLevel;
        private string key = "current_level";

        public LevelProgressManager()
        {
            if (!PlayerPrefs.HasKey(key))
                PlayerPrefs.SetInt(key, 0);
            
            ReadData();
        }

        public void ReadData()
        {
            currentLevel = PlayerPrefs.GetString(key);
        }

        public void UpdateData(string currentLevel)
        {
            this.currentLevel = currentLevel;
        }
        
        public void WriteData()
        {
            PlayerPrefs.SetString(key, currentLevel);
        }

        public string GetData()
        {
            return currentLevel;
        }
    }
}