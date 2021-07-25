using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Services.ShopData
{
    public class ShopData : MonoBehaviour
    {
        [SerializeField] private List<BoxTheme> boxThemes;
        
        
    }

    [Serializable]
    public class BoxTheme
    {
        [SerializeField] private string themeKey;
        [SerializeField] private Material material;
        
        
    }
}