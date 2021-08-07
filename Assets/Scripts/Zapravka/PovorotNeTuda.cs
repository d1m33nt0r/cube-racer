using DefaultNamespace;
using UnityEngine;

public class PovorotNeTuda : MonoBehaviour
{
    [SerializeField] private ZapravkaPovorot povorotNeTudaSpline;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DiamondCollector"))
        {
            other.transform.parent.parent.SetParent(povorotNeTudaSpline.transform.GetChild(0));
            povorotNeTudaSpline.SetMoving(true);
        }
    }
}