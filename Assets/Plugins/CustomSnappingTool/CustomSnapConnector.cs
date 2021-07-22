using UnityEngine;

public class CustomSnapConnector : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.025f);
    }
}
