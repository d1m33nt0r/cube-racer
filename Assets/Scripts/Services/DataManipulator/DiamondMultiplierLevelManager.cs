using Services.ProgressController.Interfaces;
using UnityEngine;

namespace Services.DataManipulator
{
    public class DiamondMultiplierLevelManager : IDataManipulator
    {
        private int diamondMultiplierLevel;
        private string key = "diamond_multiplier_level";

        public DiamondMultiplierLevelManager()
        {
            if (!PlayerPrefs.HasKey(key))
                PlayerPrefs.SetInt(key, 1);

            ReadData();
        }

        public void ReadData()
        {
            diamondMultiplierLevel = PlayerPrefs.GetInt(key);
        }

        public void UpdateData(int start_box_count)
        {
            this.diamondMultiplierLevel = start_box_count;
        }

        public void WriteData()
        {
            PlayerPrefs.SetInt(key, diamondMultiplierLevel);
        }

        public int GetData()
        {
            return diamondMultiplierLevel;
        }
    }
}