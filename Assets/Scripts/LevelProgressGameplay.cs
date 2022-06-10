using Services.LevelProgressManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
    public class LevelProgressGameplay : MonoBehaviour
    {
        [SerializeField] private Text levelText;

        private LevelProgressManager levelProgressManager;
        
        [Inject]
        private void Construct(LevelProgressManager levelProgressManager)
        {
            this.levelProgressManager = levelProgressManager;
        }

        private void Start()
        {
            levelText.text = levelProgressManager.currentLevelNumber.ToString();
        }
    }
}