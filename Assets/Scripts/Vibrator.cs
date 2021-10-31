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
            MMVibrationManager.Haptic(HapticTypes.Warning, false, true, this);
        }

        public void VibrateDiamond()
        {
            MMVibrationManager.Haptic(HapticTypes.Failure, false, true, this);
        }

        public void VibrateLava()
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact, false, true, this);
        }
        #endif
    }
}