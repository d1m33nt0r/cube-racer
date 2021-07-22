using System.Collections;
using UnityEngine;

public class DiamondIconController : MonoBehaviour
{
    private bool isMovingDone;
    
    public void SetMovingDone()
    {
        isMovingDone = true;
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }
    
    private IEnumerator Wait()
    {
        yield return new WaitUntil(() => isMovingDone);
        
        Destroy(gameObject);
    }
}
