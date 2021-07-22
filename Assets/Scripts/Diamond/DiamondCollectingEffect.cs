using UnityEngine;

public class DiamondCollectingEffect : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    [SerializeField] private DiamondMover diamondMover;
    [SerializeField] private Camera camera;
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
            diamondMover.CreateDiamond(camera.WorldToScreenPoint(transform.position));
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(effect).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}