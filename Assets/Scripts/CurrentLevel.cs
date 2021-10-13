using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class CurrentLevel : MonoBehaviour
    {
        public string GetNextLevelName()
        {
            var currentLevel = SceneManager.GetActiveScene().name;
            var levelNumber = Convert.ToInt32(currentLevel[currentLevel.Length - 1]);
            levelNumber++;
            return "Level_" + levelNumber;
        }
    }
}