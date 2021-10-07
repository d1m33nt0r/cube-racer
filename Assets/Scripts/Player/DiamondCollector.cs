using DefaultNamespace;
using UnityEngine;

public class DiamondCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!transform.CompareTag("DiamondCollector"))
            return;

        if (other.CompareTag("Turn"))
        {
            var playerMover = transform.parent.GetComponent<PlayerMover>();
            var rotationObserver = other.transform.parent.GetChild(0).GetComponent<RotationOberver>();

            if (other.name == "StartTrigger")
            {
                playerMover.DisableMoving();
                playerMover.transform.parent.SetParent(rotationObserver.transform);
                rotationObserver.transform.parent.GetComponent<Turn>().SetMoving(true);
            }

            if (other.name == "FinishTrigger")
            {
                playerMover.EnableMoving();
                playerMover.transform.parent.SetParent(null);
                rotationObserver.transform.parent.GetComponent<Turn>().SetMoving(false);
                playerMover.SetCurrentDirection();
            }
        }
    }
}