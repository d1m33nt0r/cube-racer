using UnityEngine;

public class RotationAnimation : MonoBehaviour
{
    private float rotation;
    [SerializeField] private Rotation type;
    [SerializeField] private float speed;

    private void Update()
    {
        switch (type)
        {
            case Rotation.Y:
                transform.rotation = Quaternion.AngleAxis(rotation += speed, Vector3.up);
                break;
            case Rotation.X:
                transform.rotation = Quaternion.AngleAxis(rotation += speed, Vector3.right);
                break;
            case Rotation.Z:
                transform.rotation = Quaternion.AngleAxis(rotation += speed, Vector3.forward);
                break;
            default:
                transform.rotation = Quaternion.AngleAxis(rotation += speed, Vector3.forward + Vector3.up + Vector3.right);
                break;
            
        }

        
        
    }

    public enum Rotation
    {
        Default,
        Y,
        X,
        Z
    }
}