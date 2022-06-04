using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Boxes
{
    public class BoxGroup : MonoBehaviour
    {
        public List<FriendlyBox> boxes;

        private Vibrator vibrator;
        private BoxAudioController boxAudioController;
        public int countBoxes => boxes.Count;
        
        public float heightBox = 0.192f;

        private const string DIAMOND_COLLECTOR_TAG = "DiamondCollector";
        
        
        private Transform startMarker;
        public Transform endMarker;

        public float speed = 1.0F;
        private float startTime;
        private float journeyLength;
        private bool ismove = true;
        
        
        [Inject]
        private void Construct(BoxAudioController boxAudioController, Vibrator _vibrator)
        {
            this.boxAudioController = boxAudioController;
            vibrator = _vibrator;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(DIAMOND_COLLECTOR_TAG))
            {
                var plusOneBoxTextEffect = new List<Vector3>();
                
                var boxController = other.transform.parent.GetComponent<BoxController>();
                var effectSpawner = boxController.GetComponent<BoxEffectSpawner>();
                var boxCount = boxController.boxCount;
                
                boxController.prevBoxCount = boxCount;
                effectSpawner.Cal(countBoxes);
                
                foreach (var box in boxes)
                {
                    boxController.AddBox(box.gameObject);
#if UNITY_ADNROID
                    vibrator.VibrateBoxes();
#endif
                }

                ismove = false;
                boxController.BoxGroupAdded(countBoxes);
            }
        }
        
        
        public void MoveToTargetTransform(Transform _transform)
        {
            startMarker = transform;
            endMarker = _transform;
            startTime = Time.time;
            journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
            //transform.DOMove(_transform.position, 0.5f);
            StartCoroutine(Move());
        }
    
        private IEnumerator Move()
        {
            while (ismove) 
            {
                //if (startMarker.childCount == 0) break;
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;
                startMarker.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
                yield return null;
            }
        }
    }
}