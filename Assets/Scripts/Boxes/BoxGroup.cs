using System.Collections.Generic;
using UI;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Boxes
{
    public class BoxGroup : MonoBehaviour
    {
        [SerializeField] private List<FriendlyBox> boxes;

        private Vibrator vibrator;
        private BoxAudioController boxAudioController;
        private int countBoxes => boxes.Count;
        
        private float heightBox = 0.2105f;

        [Inject]
        private void Construct(BoxAudioController boxAudioController, Vibrator _vibrator)
        {
            this.boxAudioController = boxAudioController;
            vibrator = _vibrator;
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
                vibrator.VibrateBoxes();
                boxAudioController.PlayCollectSound();

                effectSpawner.Cal(boxController.boxCount - boxCount);
                Debug.Log("Add " + countBoxes + " boxes");
            }
        }
    }
}