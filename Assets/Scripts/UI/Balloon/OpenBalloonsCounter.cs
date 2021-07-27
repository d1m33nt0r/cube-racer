using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBalloonsCounter : MonoBehaviour
{
    private int count = 0;

    public void AddOne()
    {
        count++;
    }

    public void ResetCounter()
    {
        count = 0;
    }
}
