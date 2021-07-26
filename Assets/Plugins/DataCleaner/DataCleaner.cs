using UnityEngine;

namespace Plugins.DataCleaner
{
    [CreateAssetMenu(fileName = "DataCleaner", menuName = "Data Cleaner", order = 0)]
    public class DataCleaner : ScriptableObject
    {
        public void CleanData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}