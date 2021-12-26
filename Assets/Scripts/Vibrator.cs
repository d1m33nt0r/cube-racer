using System;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace DefaultNamespace
{
    public class Vibrator : MonoBehaviour
    {
        private bool HapticAllowed = true;

        public void DisableHaptics()
        {
            HapticAllowed = false;
            MMVibrationManager.SetHapticsActive(HapticAllowed);
            PlayerPrefs.SetInt("vibration_enable", 0);
        }

        public void EnableHaptics()
        {
            HapticAllowed = true;
            MMVibrationManager.SetHapticsActive(HapticAllowed);
            PlayerPrefs.SetInt("vibration_enable", 1);
        }
        
        private bool GetSoundSettings()
        {
            if (PlayerPrefs.HasKey("vibration_enable"))
                return Convert.ToBoolean(PlayerPrefs.GetInt("vibration_enable"));
            
            return false;
        }
        
        public void Start()
        {
            if (GetSoundSettings())
                EnableHaptics();
            else
                DisableHaptics();
        }

        #if UNITY_ANDROID
        public void VibrateBoxes()
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }

        public void VibrateDiamond()
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }

        public void VibrateLava()
        {
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);
        }
        #endif
    }
}