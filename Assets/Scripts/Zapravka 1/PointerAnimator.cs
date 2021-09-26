using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Zapravka_1
{
    public class PointerAnimator : MonoBehaviour
    {
        [SerializeField] private float delay;
        [SerializeField] private List<Vector3> positions;

        private int currentIndex;

        private void Start()
        {
            if (positions.Count > 0) 
                StartCoroutine(Animation());
        }

        private IEnumerator Animation()
        {
            while (currentIndex < 5)
            {
                yield return new WaitForSeconds(delay);

                transform.localPosition = positions[currentIndex];

                currentIndex++;
            }
            
            currentIndex = 0;
            
            StartCoroutine(Animation());
        }
    }
}