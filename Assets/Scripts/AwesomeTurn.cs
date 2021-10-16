using System.Collections;
using PathCreation.Examples;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class AwesomeTurn : MonoBehaviour
    {
        [SerializeField] private GameObject pathFollower;

        [SerializeField] private BoxCollider startBoxCollider;
        [SerializeField] private BoxCollider finishBoxCollider;

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
                
                playerMover.DisablePhysics();
                pathFollower.GetComponent<PathFollower>().Moving += playerMover.CustomMove;
                pathFollower.GetComponent<PathFollower>().enabled = true;
                finishBoxCollider.enabled = true;
                StartCoroutine(Finish());
            }
        
            if (other.CompareTag("DiamondCollector") && finish)
            {
                playerMover.EnablePhysics();
                pathFollower.GetComponent<PathFollower>().Moving -= playerMover.CustomMove;
                playerMover.SetCurrentDirection();
                playerMover.EnableMoving();
                pathFollower.GetComponent<PathFollower>().enabled = false;
            }
        }

        private IEnumerator Finish()
        {
            yield return new WaitForSeconds(0.1f);

            finish = true;
        }
    }
}