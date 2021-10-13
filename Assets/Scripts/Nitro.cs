using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class Nitro : MonoBehaviour
{
    [SerializeField] private float multiplierSpeed;
    [SerializeField] private float duration;

    private bool isActive;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector") && !isActive)
        {
            isActive = true;
            var playerMover = other.transform.parent.GetComponent<PlayerMover>();
            playerMover.IncreaseSpeed(multiplierSpeed);
            StartCoroutine(WaitAndSetDefaultSpeed(playerMover));
            GetComponent<AudioSource>().Play();
        }
    }

    private IEnumerator WaitAndSetDefaultSpeed(PlayerMover playerMover)
    {
        yield return new WaitForSeconds(duration);
        playerMover.SetDefaultSpeed();
    }
}
