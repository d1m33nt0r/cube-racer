using Services.ProgressController.Interfaces;
using UnityEngine;

namespace Services.StartBoxCountManager
{
    public class StartBoxCountManager : IDataManipulator
    {
        private int start_box_count;
        private string key = "start_box_count";

        public StartBoxCountManager()
        {
            if (!PlayerPrefs.HasKey(key))
                PlayerPrefs.SetInt(key, 1);

            ReadData();
        }

        public void ReadData()
        {
            start_box_count = PlayerPrefs.GetInt(key);
        }

        public void UpdateData(int start_box_count)
        {
            this.start_box_count = start_box_count;
        }

        public void WriteData()
        {
            PlayerPrefs.SetInt(key, start_box_count);
        }

        public int GetData()
        {
            return start_box_count;
        }
    }
}