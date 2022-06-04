using DefaultNamespace;
using UnityEngine;

public class DiamondCollector : MonoBehaviour
{
    private const string DIAMOND_COLLECTOR_TAG = "DiamondCollector";
    private const string START_LAVA_TRIGGER_TAG = "StartLavaTrigger";
    private const string END_LAVA_TRIGGER_TAG = "EndLavaTrigger";
    private const string TURN_TAG = "Turn";
    private const string START_TURN_TRIGGER = "StartTrigger";
    private const string END_TURN_TRIGGER = "FinishTrigger";
    
    private void OnTriggerEnter(Collider other)
    {
        if (!transform.CompareTag(DIAMOND_COLLECTOR_TAG))
            return;

        if (other.CompareTag(START_LAVA_TRIGGER_TAG))
        {
            Physics.gravity = new Vector3(0, -22, 0);
            return;
        }
        
        if (other.CompareTag(END_LAVA_TRIGGER_TAG))
        {
            Physics.gravity = new Vector3(0, -35, 0);
            return;
        }
        
        if (other.CompareTag(TURN_TAG))
        {
            var playerMover = transform.parent.GetComponent<PlayerMover>();
            var rotationObserver = other.transform.parent.GetChild(0).GetComponent<RotationOberver>();

            if (other.name == START_TURN_TRIGGER)
            {
                playerMover.DisableMoving();
                playerMover.transform.parent.SetParent(rotationObserver.transform);
                rotationObserver.transform.parent.GetComponent<Turn>().SetMoving(true);
            }

            if (other.name == END_TURN_TRIGGER)
            {
                playerMover.EnableMoving();
                playerMover.transform.parent.SetParent(null);
                rotationObserver.transform.parent.GetComponent<Turn>().SetMoving(false);
                playerMover.SetCurrentDirection();
            }
        }
    }
}