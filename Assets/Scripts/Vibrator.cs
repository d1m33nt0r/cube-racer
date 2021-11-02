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
            MMVibrationManager.TransientHaptic(0.01f, 0.05f);
        }

        public void VibrateDiamond()
        {
            MMVibrationManager.TransientHaptic(0.05f, 0.1f);
        }

        public void VibrateLava()
        {
            MMVibrationManager.TransientHaptic(0.2f, 0.5f);
        }
        #endif
    }
}