using DefaultNamespace;
using UnityEngine;
using Zenject;

public class FriendlyBox : MonoBehaviour
{
    private Rigidbody rb;
    private BoxController boxController;
    private PhysicsManipulator physicsManipulator;
    
    [Inject]
    private void Construct(BoxController boxController, PhysicsManipulator physicsManipulator)
    {
        this.boxController = boxController;
        this.physicsManipulator = physicsManipulator;
    }
    
    private void Start() => rb = GetComponent<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FriendlyBox"))
        {
            other.tag = "Untagged";
            boxController.AddBox(other.gameObject);
        }

        if (other.CompareTag("LetBox"))
        {
            transform.SetParent(null); 
            physicsManipulator.EnablePhysics();
        }

        if (other.CompareTag("Untagged"))
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(0,0,0);
            //rb.isKinematic = true;
            //rb.isKinematic = false;
            
        }

        if (other.CompareTag("Ground"))
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(0,0,0);
            //rb.isKinematic = true;
            //rb.isKinematic = false;
        }
    }
}
