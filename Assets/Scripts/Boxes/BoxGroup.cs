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
                List<Vector3> collectBoxEffectPositions = new List<Vector3>();
                List<Vector3> plusOneBoxTextEffect = new List<Vector3>();

                var boxController = other.transform.parent.GetComponent<BoxController>();
                var effectSpawner = boxController.GetComponent<PlusOneFriendlyBoxAnimationController>();
                var boxCount = boxController.boxCount;

                var pos = boxController.GetBoxPositionXZ();
                var pos2 = boxController.transform.TransformPoint(StartPoint.startPoint);
                var position = boxController.transform.TransformPoint(pos);
                
                //foreach (var box in boxes)
                //{
                    
                //    collectBoxEffectPositions.Add(new Vector3(position.x, pos2.y + boxCount * 0.2f, position.z));
                //}
                
                foreach (var box in boxes)
                {
                    other.transform.parent.GetComponent<BoxController>().AddBox(box.gameObject);
                }

                ///foreach (var effectPosition in collectBoxEffectPositions)
                ///{
                ///    effectSpawner.SpawnEffect(effectPosition);
                //}

                boxAudioController.PlayCollectSound();
                
                boxController.CalculateBoxPositions();
                
                Debug.Log("Add " + countBoxes + " boxes");
            }
        }
    }
}