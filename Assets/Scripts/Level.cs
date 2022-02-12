using UnityEngine;

namespace DefaultNamespace
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private string nextLevel;
        [SerializeField] private string currentLevel;

        public string NextLevel => nextLevel;
        public string CurrentLevel => currentLevel;

        /*public void Start()
        {
            Firebase.Analytics.FirebaseAnalytics
                .LogEvent(currentLevel);
        }*/
    }
}