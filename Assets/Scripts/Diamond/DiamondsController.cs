using UnityEngine;

namespace Diamond
{
    public class DiamondsController : MonoBehaviour
    {
        private void Start()
        {
            var effects = transform.GetComponentsInChildren<DiamondCollectingEffect>();
            foreach (var effect in effects)
            {
                effect.BindAudioSource(GetComponent<AudioSource>());
            }
        }
    }
}