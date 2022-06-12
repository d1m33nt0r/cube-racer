using System.Collections.Generic;
using Services.LevelProgressManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
    public class LevelProgressMenu : MonoBehaviour
    {
        [SerializeField] private Color lockedLevelColor = new Color(1,1,1,1);
        [SerializeField] private Color currentLevelColor = new Color(1,1,1,1);
        [SerializeField] private Color completedLevelColor = new Color(1,1,1,1);

        [SerializeField] private List<Text> levelTexts;
        [SerializeField] private List<Image> levelImages;

        private LevelProgressManager levelProgressManager;
        
        [Inject]
        private void Construct(LevelProgressManager levelProgressManager)
        {
            this.levelProgressManager = levelProgressManager;
        }
        
        private void Start()
        {
            var hz = Mathf.FloorToInt(levelProgressManager.currentLevelNumber / 6);
            var ostatok = levelProgressManager.currentLevelNumber % 6;
            
            if (ostatok == 0)
            {
                levelTexts[0].text = ((hz - 1) * 6 + 1).ToString();
                levelImages[0].color = completedLevelColor;
                
                levelTexts[1].text = ((hz - 1) * 6 + 2).ToString();
                levelImages[1].color = completedLevelColor;
                
                levelTexts[2].text = ((hz - 1) * 6 + 3).ToString(); 
                levelImages[2].color = completedLevelColor;
                
                levelTexts[3].text = ((hz - 1) * 6 + 4).ToString();
                levelImages[3].color = completedLevelColor;
              
                levelTexts[4].text = ((hz - 1) * 6 + 5).ToString();
                levelImages[4].color = completedLevelColor;
            }
            else
            {
                levelTexts[0].text = (hz * 6 + 1).ToString();
                if (hz * 6 + 1 < hz * 6 + ostatok) levelImages[0].color = completedLevelColor;
                else if (hz * 6 + 1 == hz * 6 + ostatok) levelImages[0].color = currentLevelColor;
                else levelImages[0].color = lockedLevelColor;
                    
                levelTexts[1].text = (hz * 6 + 2).ToString();
                if (hz * 6 + 2 < hz * 6 + ostatok) levelImages[1].color = completedLevelColor;
                else if (hz * 6 + 2 == hz * 6 + ostatok) levelImages[1].color = currentLevelColor;
                else levelImages[1].color = lockedLevelColor;
                
                levelTexts[2].text = (hz * 6 + 3).ToString();
                if (hz * 6 + 3 < hz * 6 + ostatok) levelImages[2].color = completedLevelColor;
                else if (hz * 6 + 3 == hz * 6 + ostatok) levelImages[2].color = currentLevelColor;
                else levelImages[2].color = lockedLevelColor;
                
                levelTexts[3].text = (hz * 6 + 4).ToString();
                if (hz * 6 + 4 < hz * 6 + ostatok) levelImages[3].color = completedLevelColor;
                else if (hz * 6 + 4 == hz * 6 + ostatok) levelImages[3].color = currentLevelColor;
                else levelImages[3].color = lockedLevelColor;
              
                levelTexts[4].text = (hz * 6 + 5).ToString();
                if (hz * 6 + 5 < hz * 6 + ostatok) levelImages[4].color = completedLevelColor;
                else if (hz * 6 + 5 == hz * 6 + ostatok) levelImages[4].color = currentLevelColor;
                else levelImages[4].color = lockedLevelColor;
            }
        }
    }
}