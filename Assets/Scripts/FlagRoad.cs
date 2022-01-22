using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using Zenject;

public class FlagRoad : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private AudioManager m_audioManager;
    
    [Inject]
    private void Construct(AudioManager _audioManager)
    {
        m_audioManager = _audioManager;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector"))
        {
            animator.enabled = true;
            animator.Play("FlagAnim");
            m_audioManager.flagEffectAudioSource.PlayFlagEffectSound();
        }
    }
    
}
