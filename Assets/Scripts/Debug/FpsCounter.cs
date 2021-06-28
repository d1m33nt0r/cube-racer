using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    public int FPS { get; private set; }

    private void Update()
    {
        FPS = (int)(1f / Time.deltaTime);
    }
}
