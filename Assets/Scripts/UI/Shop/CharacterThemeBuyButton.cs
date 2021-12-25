using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Services.AudioManager;
using DefaultNamespace.ThemeManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Shop
{
    public class CharacterThemeBuyButton : MonoBehaviour
    {
        private List<CharacterThemeButton> buttons => transform.parent.GetComponent<CharacterButtonsConstructor>().ThemeButtons;

        private ThemeManager themeManager;

        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite notSelectedSprite;
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(ThemeManager themeManager, AudioManager _audioManager)
        {
            this.themeManager = themeManager;
            m_audioManager = _audioManager;
        }

        public void BuyRandomCharacter()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            var themeButtons = buttons.Where(button => !button.characterTheme.bought).ToList();
            var randNum = Random.Range(0, themeButtons.Count - 1);
            StartCoroutine(Randomize(themeButtons));
        }

        private IEnumerator Randomize(List<CharacterThemeButton> characterThemeButtons)
        {
            var pointer = new PointerEventData(EventSystem.current);
            var randNum = 0;
            var prevNum = 0;
            var i = 0;

            while (i < 12)
            {
                if (characterThemeButtons.Count < 2)
                {
                    randNum = 0;
                }
                else
                {
                    while (prevNum == randNum)
                    {
                        randNum = Random.Range(0, characterThemeButtons.Count);
                    }
                }

                foreach (var button in characterThemeButtons)
                {
                    ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerUpHandler);
                }

                if (characterThemeButtons.Count < 2)
                {
                    i = 11;
                }

                if (i == 11)
                {
                    foreach (var button in buttons)
                    {
                        button.GetComponent<Image>().sprite = notSelectedSprite;
                    }

                    characterThemeButtons[randNum].GetComponent<Image>().sprite = selectedSprite;
                    themeManager.BuyCharacterTheme(characterThemeButtons[randNum].characterTheme.key);
                    themeManager.SetCurrentCharacterTheme(characterThemeButtons[randNum].characterTheme.key);
                    characterThemeButtons[randNum].characterTheme.bought = true;
                    characterThemeButtons[randNum].ActiveDemoCharacter();
                }
                else
                {
                    ExecuteEvents.Execute(characterThemeButtons[randNum].gameObject, pointer,
                        ExecuteEvents.pointerDownHandler);
                }


                prevNum = randNum;
                i++;
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}