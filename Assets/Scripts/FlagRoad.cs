using UnityEngine;

public class FlagRoad : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("DiamondCollector") || other.collider.CompareTag("Untagged"))
        {
            animator.Play("FlagAnim");
        }
    }
}
