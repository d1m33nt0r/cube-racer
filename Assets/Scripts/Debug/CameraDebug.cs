using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDebug : MonoBehaviour
{
    [SerializeField] private float value;
    
    private float minimum = 0;
    private float maximum = 1f;
    private float t = 0.0f;
    
    public void ScaleFieldView()
    {
        StartCoroutine(Scaling());
    }

    private IEnumerator Scaling()
    {
        var working = true;
        
        while (working)
        {
            GetComponent<Camera>().fieldOfView += Mathf.Lerp(0, 1, t);
            
            t += 0.5f * Time.deltaTime;
            
            if (t > 1.0f)
            {
                working = false;
                t = 0.0f;
            }
            
            yield return null;
        }
    }
}
