using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class Nitro : MonoBehaviour
{
    [SerializeField] private float multiplierSpeed;
    [SerializeField] private float duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector"))
        {
            var playerMover = other.transform.parent.GetComponent<PlayerMover>();
            playerMover.IncreaseSpeed(multiplierSpeed);
            StartCoroutine(WaitAndSetDefaultSpeed(playerMover));
        }
    }

    private IEnumerator WaitAndSetDefaultSpeed(PlayerMover playerMover)
    {
        yield return new WaitForSeconds(duration);
        playerMover.SetDefaultSpeed();
    }
}
