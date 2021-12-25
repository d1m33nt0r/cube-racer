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
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("DiamondCollector") || other.collider.CompareTag("Untagged"))
        {
            animator.enabled = true;
            animator.Play("FlagAnim");
            m_audioManager.flagEffectAudioSource.PlayFlagEffectSound();
        }
    }
}
