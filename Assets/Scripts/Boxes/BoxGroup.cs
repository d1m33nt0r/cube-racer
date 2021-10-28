using System.Collections.Generic;
using UI;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Boxes
{
    public class BoxGroup : MonoBehaviour
    {
        [SerializeField] private List<FriendlyBox> boxes;

        private BoxAudioController boxAudioController;
        private int countBoxes => boxes.Count;

        private StartingRoad startingRoad;
        private float heightBox = 0.2105f;
        
        private Bounds GroundBounds => startingRoad.GetComponent<MeshRenderer>().bounds;
        private float offsetYForGround => Mathf.Abs(GroundBounds.max.y - GroundBounds.center.y);
        
        [Inject]
        private void Construct(BoxAudioController boxAudioController)
        {
            this.boxAudioController = boxAudioController;
        }

        private void Awake()
        {
            startingRoad = FindObjectOfType<StartingRoad>();
            CalculatePositions();
        }

        private void CalculatePositions()
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                if (i == transform.childCount - 1)
                {
                    var box = transform.GetChild(transform.childCount - 1);
                    box.position = new Vector3(transform.position.x,
                        startingRoad.transform.position.y + offsetYForGround + heightBox / 2, transform.position.z);

                    box.tag = "Untagged";
                }
                else
                {
                    var prevBoxIndex = i + 1;
                    var box = transform.GetChild(i);
                    box.position = new Vector3(transform.position.x,
                        transform.GetChild(prevBoxIndex).position.y + heightBox, transform.position.z);

                    box.tag = "Untagged";
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DiamondCollector"))
            {
                List<Vector3> plusOneBoxTextEffect = new List<Vector3>();

                var boxController = other.transform.parent.GetComponent<BoxController>();
                var effectSpawner = boxController.GetComponent<BoxEffectSpawner>();
                var boxCount = boxController.boxCount;
                
                boxController.prevBoxCount = boxCount;
                
                foreach (var box in boxes)
                    boxController.AddBox(box.gameObject);

                boxController.BoxGroupAdded(countBoxes);

                boxAudioController.PlayCollectSound();
                
                boxController.CalculateBoxPositions();
                
                effectSpawner.Cal(boxController.boxCount - boxCount);
                Debug.Log("Add " + countBoxes + " boxes");
            }
        }
    }
}