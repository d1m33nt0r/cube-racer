using UnityEngine;

public class RotationAnimation : MonoBehaviour
{
    private float rotation;
    [SerializeField] private bool osj;
    [SerializeField] private float speed;
    
    private void Update()
    {
        if (osj)
            transform.rotation = Quaternion.AngleAxis(rotation += speed, Vector3.up + Vector3.forward + Vector3.right);
        else
            transform.rotation = Quaternion.AngleAxis(rotation += speed, Vector3.forward);
    }
}
