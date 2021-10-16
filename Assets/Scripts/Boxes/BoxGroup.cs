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

        [Inject]
        private void Construct(BoxAudioController boxAudioController)
        {
            this.boxAudioController = boxAudioController;
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