using DefaultNamespace;
using PathCreation.Examples;
using UnityEngine;

public class PovorotNeTuda : MonoBehaviour
{
    [SerializeField] private ZapravkaPovorot povorotNeTudaSpline;

    [SerializeField] private GameObject pathFollower;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector"))
        {
            
            //other.transform.parent.parent.SetParent(pathFollower.transform);
            //other.transform.parent.GetComponent<PlayerMover>().DisableMoving();
            pathFollower.GetComponent<PathFollower>().enabled = true;
            //povorotNeTudaSpline.SetMoving(true);
        }
    }
}