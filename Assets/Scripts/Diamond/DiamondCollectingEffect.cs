using UI.GlobalUI.DiamondCounter;
using UnityEngine;
using Zenject;

public class DiamondCollectingEffect : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    [SerializeField] private DiamondMover diamondMover;
    [SerializeField] private Camera camera;
    
    private AudioSource audioSource;
    private DiamondCounter diamondCounter;
    
    [Inject]
    private void Construct(DiamondCounter diamondCounter)
    {
        this.diamondCounter = diamondCounter;
    }
    
    public void BindAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector"))
        {
            audioSource.Play();
            diamondCounter.AddDiamond();
            diamondMover.CreateDiamond(camera.WorldToScreenPoint(transform.position));
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(effect).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}