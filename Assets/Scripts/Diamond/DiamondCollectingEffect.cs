using UnityEngine;

public class DiamondCollectingEffect : MonoBehaviour
{
    [SerializeField] private GameObject effect;

    private AudioSource audioSource;

    public void BindAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Untagged"))
        {
            audioSource.Play();
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(effect).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}