using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using Zenject;

public class Nitro : MonoBehaviour
{
    [SerializeField] private float multiplierSpeed;
    [SerializeField] private float duration;

    private bool isActive;
    private AudioManager m_audioManager;
    
    [Inject]
    private void Construct(AudioManager _audioManager)
    {
        m_audioManager = _audioManager;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector") && !isActive)
        {
            isActive = true;
            var playerMover = other.transform.parent.GetComponent<PlayerMover>();
            playerMover.IncreaseSpeed(multiplierSpeed);
            StartCoroutine(WaitAndSetDefaultSpeed(playerMover));
            m_audioManager.nitroAudioEffectSource.PlayNitroSound();
        }
    }

    private IEnumerator WaitAndSetDefaultSpeed(PlayerMover playerMover)
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(playerMover.SetDefaultSpeed());
    }
}
