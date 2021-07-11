using System;
using UnityEngine;

public class Box : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "FriendlyBox")
        {
            
        }
    }
}
