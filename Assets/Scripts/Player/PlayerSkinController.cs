using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class PlayerSkinController : MonoBehaviour
    {
        private ThemeManager.ThemeManager themeManager;
        
        [Inject]
        private void Construct(ThemeManager.ThemeManager _themeManager)
        {
            themeManager = _themeManager;
        }
        
        private void Start()
        {
            GetCurrentMaterial();
        }

        private void GetCurrentMaterial()
        {
            GetComponent<MeshFilter>().sharedMesh = themeManager.GetCurrentCharacterTheme();
        }
    }
}