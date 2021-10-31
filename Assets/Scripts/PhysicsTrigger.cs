using UnityEngine;

public class PhysicsTrigger : MonoBehaviour
{
    [SerializeField] private bool isDisableTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DiamondCollector")) return;
        
        //if (isDisableTrigger)
          //  CalculatePositionsAndDisable(other);
        //else
        //   other.transform.parent.GetComponent<BoxController>().EnablePhysics();
    }

    private static void CalculatePositionsAndDisable(Collider other)
    {
        var boxController = other.transform.parent.GetComponent<BoxController>();
        //boxController.CalculateBoxPositions(); 
        //boxController.DisablePhysics();
    }
}
