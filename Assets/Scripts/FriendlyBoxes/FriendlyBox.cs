using UnityEngine;
using Zenject;

public class FriendlyBox : MonoBehaviour
{
    private BoxController boxController;

    [Inject] private void Construct(BoxController boxController) => this.boxController = boxController;

    private void OnCollisionEnter(Collision other)
    {
        if (!transform.parent || transform.parent.name != "Player")
            return;

        if (other.collider.CompareTag("Untagged"))
            return;
            
        if (other.collider.CompareTag("FriendlyBox"))
        {
            other.collider.tag = "Untagged";
            boxController.AddBox(other.gameObject);
            boxController.DisablePhysics();
        }

        if (other.collider.CompareTag("LetBox"))
        {
            boxController.RemoveBox(gameObject);
            boxController.EnablePhysics(true);
        }

        if (other.collider.CompareTag("Ground"))
        {
            boxController.UpdateTrailPosition();
        }
    }
}