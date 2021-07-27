using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class FInishMusor : MonoBehaviour
    {
        private GameController gameController;

        [Inject]
        private void Construct(GameController gameController)
        {
            this.gameController = gameController;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player") || other.collider.CompareTag("Untagged") || other.collider.CompareTag("DiamondCollector"))
            {
                GetComponent<BoxCollider>().enabled = false;
                
                //other.collider.GetComponent<Rigidbody>().useGravity = true;
                gameController.FinishGame();
            }
        }
    }
}