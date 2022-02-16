using System.Collections;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace DefaultNamespace
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private string nextLevel;
        [SerializeField] private string currentLevel;

        public string NextLevel => nextLevel;
        public string CurrentLevel => currentLevel;

        public void Start()
        {
            FirebaseAnalyticsInitialize.CheckIfReady();
            StartCoroutine(SendTestEvent());
        }

        private IEnumerator SendTestEvent()
        {
            while (!FirebaseAnalyticsInitialize.firebaseReady)
            {
                yield return null;
            }

            FirebaseAnalytics.LogEvent(currentLevel, "Start", "Start");
        }
    }
}