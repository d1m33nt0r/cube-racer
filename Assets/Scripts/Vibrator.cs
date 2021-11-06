using MoreMountains.NiceVibrations;
using UnityEngine;

namespace DefaultNamespace
{
    public class Vibrator : MonoBehaviour
    {
        private bool HapticAllowed = true;

        public void Start()
        {
            MMVibrationManager.SetHapticsActive(HapticAllowed);
        }

        #if UNITY_ANDROID
        public void VibrateBoxes()
        {
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);
        }

        public void VibrateDiamond()
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }

        public void VibrateLava()
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
        #endif
    }
}