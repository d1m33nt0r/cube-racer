using Services.DiamondCountManager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class FInishMusor : MonoBehaviour
    {
        private GameController gameController;
        private DiamondCountManager diamondCountManager;
        private SessionDiamondCounter sessionDiamondCounter;
        private DiamondMultiplier diamondMultiplier;
        
        [Inject]
        private void Construct(GameController gameController, SessionDiamondCounter sessionDiamondCounter, 
            DiamondCountManager diamondCountManager, DiamondMultiplier diamondMultiplier)
        {
            this.gameController = gameController;
            this.diamondCountManager = diamondCountManager;
            this.sessionDiamondCounter = sessionDiamondCounter;
            this.diamondMultiplier = diamondMultiplier;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player") || other.collider.CompareTag("Untagged") || other.collider.CompareTag("DiamondCollector"))
            {
                GetComponent<BoxCollider>().enabled = false;
                
                gameController.FinishGame();
            }
        }
    }
}