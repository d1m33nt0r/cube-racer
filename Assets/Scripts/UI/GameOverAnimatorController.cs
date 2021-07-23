using UnityEngine;

public class GameOverAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        animator.Play("Show");
    }
}
