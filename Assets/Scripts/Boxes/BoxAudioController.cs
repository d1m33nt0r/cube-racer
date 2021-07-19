using UnityEngine;

public class BoxAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip failSound;
    
    public void PlayCollectSound()
    {
        audioSource.clip = collectSound;
        audioSource.Play();    
    }

    public void PlayFailSound()
    {
        audioSource.clip = failSound;
        audioSource.Play();    
    }
}
