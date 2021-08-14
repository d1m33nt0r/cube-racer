using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TrailMover : MonoBehaviour
{
    void Update()
    {
        transform.position =
            new Vector3(transform.parent.GetComponentInChildren<PlayerMover>().transform.position.x, 
                transform.position.y, transform.parent.GetComponentInChildren<PlayerMover>().transform.position.z);
    }
}
