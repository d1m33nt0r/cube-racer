using UnityEngine;

namespace DefaultNamespace
{
    public class BonusEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effect;

        public void Play()
        {
            effect.Play();
        }
    }
}