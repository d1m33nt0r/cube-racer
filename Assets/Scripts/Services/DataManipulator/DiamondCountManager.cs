using Services.ProgressController.Interfaces;
using UnityEngine;

namespace Services.DiamondCountManager
{
    public class DiamondCountManager : IDataManipulator
    {
        private int countDiamonds;
        private string key = "count_diamonds";

        public DiamondCountManager()
        {
            if (!PlayerPrefs.HasKey(key))
                PlayerPrefs.SetInt(key, 0);
            
            ReadData();
        }

        public void ReadData()
        {
            countDiamonds = PlayerPrefs.GetInt(key);
        }

        public void UpdateData(int countDiamonds)
        {
            this.countDiamonds = countDiamonds;
        }
        
        public void WriteData()
        {
            PlayerPrefs.SetInt(key, countDiamonds);
        }

        public int GetData()
        {
            return countDiamonds;
        }
    }
}