using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using Zenject;

public class Magnit : MonoBehaviour
{
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
            other.transform.parent.parent.GetComponentInChildren<MagnitPlayer>().EnableMagnitPlayerAndDestroyMagnitOnMap(gameObject);
            m_audioManager.bonusAudioSource.PlayBonusAudioSound();
        }
    }
}
