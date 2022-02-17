using System;
using UnityEngine;

namespace DefaultNamespace.Services.ShopData
{
    [Serializable]
    public class CharacterTheme
    {
        public string key;
        public GameObject gameObject;
        public bool bought;
    }
}