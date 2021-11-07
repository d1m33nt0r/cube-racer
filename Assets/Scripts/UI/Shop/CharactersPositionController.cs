using System.Collections.Generic;
using UnityEngine;

namespace UI.Shop
{
    public class CharactersPositionController : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> buttonsRect;
        [SerializeField] private List<Transform> characters;

        [SerializeField] private SerializableButton buttons;

        private void Start()
        {
            if (buttons.Count > 0) return;

            for (var i = 0; i < buttonsRect.Count; i++)
                buttons.Add(buttonsRect[i], characters[i]);
            
            var camera = GameObject.Find("Main Camera").GetComponent<Camera>();

            foreach (var keyValue in buttons)
                keyValue.Value.position = RectTransformUtility.PixelAdjustPoint(keyValue.Key.position,
                    keyValue.Key, GetComponent<Canvas>());
        }
    }
}