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
           
            int layerMask = 1 << 10;

            RaycastHit hit;
            
            var multiplier = 1;
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
            {
                multiplier = hit.collider.GetComponent<FinishPlatform>().Multiplier;
            }
            
            diamondMultiplier.SetMultiplier(multiplier);
        }
    }
}