using UnityEngine;

namespace Diamond
{
    public class DiamondAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void Play()
        {
            audioSource.Play();
        }
    }
}