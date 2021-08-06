using UnityEngine;

public class PovorotNeTuda : MonoBehaviour
{
    [SerializeField] private PovorotNeTudaSpline povorotNeTudaSpline;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector"))
        {
            other.transform.parent.parent.SetParent(povorotNeTudaSpline.transform.GetChild(0));
            povorotNeTudaSpline.SetMoving(true);
        }
    }
}