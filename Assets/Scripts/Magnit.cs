using UnityEngine;

public class Magnit : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioSource audioSource;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector"))
        {
            other.transform.parent.parent.GetComponentInChildren<MagnitPlayer>().EnableMagnitPlayerAndDestroyMagnitOnMap(gameObject);
        }
    }

    private void PlayCollectSound()
    {
        audioSource.clip = collectSound;
        audioSource.Play();
    }
}
