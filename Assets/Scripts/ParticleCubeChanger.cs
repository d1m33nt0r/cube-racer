using System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class ParticleCubeChanger : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_particleSystem;

        [Inject]
        private void Construct(ThemeManager.ThemeManager _themeManager)
        {
            m_particleSystem.GetComponent<Renderer>().material = _themeManager.GetCurrentBoxTheme();
        }
    }
}