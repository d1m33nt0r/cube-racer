using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class FinishChecker : MonoBehaviour
    {
        private DiamondMultiplier diamondMultiplier;

        [Inject]
        private void Construct(GameController gameController, DiamondMultiplier diamondMultiplier)
        {
            gameController.FinishedGame += SetMultiplier;
            this.diamondMultiplier = diamondMultiplier;
        }

        private void SetMultiplier()
        {
            RaycastHit hit;
            
            var multiplier = 1;
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                multiplier = hit.collider.GetComponentInChildren<FinishPlatform>().Multiplier;
            }
            
            diamondMultiplier.SetMultiplier(multiplier);
        }
    }
}