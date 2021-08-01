using UnityEngine;

namespace DefaultNamespace
{
    public class RotationOberver : MonoBehaviour
    {
        public delegate void DeltaRotationAction(float delta);
        public delegate void DeltaPositionAction(Vector3 delta);

        public DeltaPositionAction ChangedPosition;
        public DeltaRotationAction ChangedYRotation;

        private Vector3 previousPosition;
        private float previousYRotation;

        private void Update()
        {
            if (previousPosition != transform.position)
            {
                ChangedPosition?.Invoke(new Vector3(transform.position.x - previousPosition.x,
                    transform.position.y, transform.position.z - previousPosition.z));
            }

            if (previousYRotation != transform.rotation.y)
            {
                ChangedYRotation?.Invoke(Mathf.Abs(transform.rotation.y - previousYRotation));
            }

            previousPosition = transform.position;
            previousYRotation = transform.rotation.y;
        }
    }
}