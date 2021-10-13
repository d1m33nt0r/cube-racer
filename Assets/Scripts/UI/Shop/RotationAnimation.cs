using System;
using UnityEngine;

public class RotationAnimation : MonoBehaviour
{
    private float rotation;
    [SerializeField] private Rotation type;
    [SerializeField] private float speed;
    
    [SerializeField] private float angle;
    [SerializeField] private Vector3 vector;

    private float angle1;
    private Quaternion q;

    private void Start()
    {
        angle1 = angle * (Mathf.PI / 180);
        vector = vector.normalized;
        q = new Quaternion(Mathf.Sin(angle1 / 2) * vector.x, Mathf.Sin(angle1 / 2) * vector.y,
            Mathf.Sin(angle1 / 2) * vector.z, Mathf.Cos(angle1 / 2));
    }

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
                angle1 = angle * (Mathf.PI / 180);
                vector = vector.normalized;
                q = new Quaternion(Mathf.Sin(angle1 / 2) * vector.x, Mathf.Sin(angle1 / 2) * vector.y,
                    Mathf.Sin(angle1 / 2) * vector.z, Mathf.Cos(angle1 / 2));
                transform.rotation = transform.rotation * q;
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