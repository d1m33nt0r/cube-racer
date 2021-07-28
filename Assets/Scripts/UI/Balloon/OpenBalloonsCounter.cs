using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBalloonsCounter : MonoBehaviour
{
    private int count = 0;

    public void AddOne()
    {
        count++;
        
        if (count == 3)
            GetComponent<Animator>().Play("ShowUI");
    }

    public int GetCount()
    {
        return count;
    }
    
    public void ResetCounter()
    {
        count = 0;
    }
}
