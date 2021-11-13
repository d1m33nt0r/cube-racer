using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PositionLerper : MonoBehaviour
    {
        private Vector3 startMarker;
        private Vector3 endMarker;
        private float speed = 1.0F;
        private float startTime;
        private float journeyLength;
        private Coroutine lerping;
        
        public void Lerp(Vector3 _startMarker, Vector3 _endMarker)
        {
            if (lerping != null)
            {
                StopCoroutine(lerping);
                lerping = null;
            }
            
            startMarker = _startMarker;
            endMarker = _endMarker;

            startTime = Time.time;
            journeyLength = Vector3.Distance(startMarker, endMarker);
            lerping = StartCoroutine(Lerping());
        }
        
        private IEnumerator Lerping()
        {
            while (true)
            {
                yield return null;
                
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
                
                if (fractionOfJourney.Equals(journeyLength)) break;
            }
        }
    }
}