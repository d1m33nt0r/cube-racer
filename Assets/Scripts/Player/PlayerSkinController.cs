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
            var player = Instantiate(themeManager.GetCurrentCharacterTheme(), transform.position, Quaternion.identity, transform);
            player.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
    }
}