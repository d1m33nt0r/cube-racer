using System.Collections;
using DefaultNamespace;
using PathCreation.Examples;
using UnityEngine;
using Zenject;

public class Zapravka : MonoBehaviour
{
    [SerializeField] private GameObject pathFollower;

    private bool finish;
    private PlayerMover playerMover;

    [Inject]
    private void Construct(PlayerMover playerMover)
    {
        this.playerMover = playerMover;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector") && !finish)
        {
            playerMover.DisableMoving();
            playerMover.UnsubscribeSwipes();
            playerMover.DisablePhysics();
            pathFollower.GetComponent<PathFollower>().Moving += playerMover.CustomMove;
            pathFollower.GetComponent<PathFollower>().enabled = true;
            StartCoroutine(Finish());
        }
        
        if (other.CompareTag("DiamondCollector") && finish)
        {
            playerMover.EnablePhysics();
            pathFollower.GetComponent<PathFollower>().Moving -= playerMover.CustomMove;
            playerMover.SetCurrentDirection();
            playerMover.EnableMoving();
            playerMover.SubscribeSwipes();
            pathFollower.GetComponent<PathFollower>().enabled = false;
        }
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(1);

        finish = true;
    }
}
