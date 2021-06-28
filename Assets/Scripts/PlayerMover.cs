using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform directionPoint;

        private Vector3 direction => directionPoint.TransformPoint(directionPoint.position) -
                                     transform.TransformPoint(transform.position);

        public void Rotate(float angle)
        {
            transform.rotation = Quaternion.AngleAxis(angle - transform.rotation.eulerAngles.y, Vector3.up);
            Debug.Log(true);
        }
        
        private void Update()
        {
            Debug.DrawRay(transform.position, direction, Color.yellow);
            transform.Translate(direction * Time.deltaTime * _speed);
            //_meshScaner.Move(transform.position);
        }
    }
}