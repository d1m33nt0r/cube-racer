using UnityEngine;

public class BoxAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip collectSound2;
    [SerializeField] private AudioClip failSound;
    
    public void PlayCollectSound()
    {
        if (Random.Range(1, 3) == 1)
            audioSource.clip = collectSound;
        else
            audioSource.clip = collectSound2;
        
        audioSource.Play();    
    }

    public void PlayFailSound()
    {
        audioSource.clip = failSound;
        audioSource.Play();    
    }
}
