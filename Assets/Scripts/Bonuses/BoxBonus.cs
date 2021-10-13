using UnityEngine;

public class BoxBonus : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
        
    }
}
