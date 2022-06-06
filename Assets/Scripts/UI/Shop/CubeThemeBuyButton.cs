using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Services.AudioManager;
using DefaultNamespace.ThemeManager;
using Services.DiamondCountManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Shop
{
    public class CubeThemeBuyButton : MonoBehaviour
    {
        private List<CubeThemeButton> buttons => transform.parent.GetComponent<CubeButtonsConstructor>().ThemeButtons;

        private ThemeManager themeManager;

        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite notSelectedSprite;
        private DiamondCountManager _diamondCountManager;
        
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(ThemeManager themeManager, AudioManager _audioManager, DiamondCountManager _diamondCountManager)
        {
            this.themeManager = themeManager;
            m_audioManager = _audioManager;
            this._diamondCountManager = _diamondCountManager;
        }

        public void ByRandomCube()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            if (_diamondCountManager.GetDiamondCount() < 5000) return;
            _diamondCountManager.UpdateData(_diamondCountManager.GetDiamondCount() - 5000);
            _diamondCountManager.WriteData();
            var themeButtons = buttons.Where(button => !button.boxTheme.bought).ToList();
            var randNum = Random.Range(0, themeButtons.Count - 1);
            StartCoroutine(Randomize(themeButtons));
        }

        private IEnumerator Randomize(List<CubeThemeButton> cubeThemeButtons)
        {
            var pointer = new PointerEventData(EventSystem.current);
            var randNum = 0;
            var prevNum = 0;
            var i = 0;

            while (i < 12)
            {
                if (cubeThemeButtons.Count < 2)
                {
                    randNum = 0;
                }
                else
                {
                    while (prevNum == randNum)
                    {
                        randNum = Random.Range(0, cubeThemeButtons.Count);
                    }
                }

                foreach (var button in cubeThemeButtons)
                {
                    ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerUpHandler);
                }

                if (cubeThemeButtons.Count < 2)
                {
                    i = 11;
                }

                if (i == 11)
                {
                    foreach (var button in buttons)
                    {
                        button.GetComponent<Image>().sprite = notSelectedSprite;
                    }

                    cubeThemeButtons[randNum].GetComponent<Image>().sprite = selectedSprite;
                    themeManager.BuyCubeTheme(cubeThemeButtons[randNum].boxTheme.key);
                    themeManager.SetCurrentBoxTheme(cubeThemeButtons[randNum].boxTheme.key);
                    cubeThemeButtons[randNum].boxTheme.bought = true;
                    cubeThemeButtons[randNum].ActiveDemoCube();
                }
                else
                {
                    ExecuteEvents.Execute(cubeThemeButtons[randNum].gameObject, pointer,
                        ExecuteEvents.pointerDownHandler);
                }


                prevNum = randNum;
                i++;
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}