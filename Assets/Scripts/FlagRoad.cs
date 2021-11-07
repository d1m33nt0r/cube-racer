using UnityEngine;

public class FlagRoad : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("DiamondCollector") || other.collider.CompareTag("Untagged"))
        {
            animator.enabled = true;
            animator.Play("FlagAnim");
            audioSource.Play();
        }
    }
}
