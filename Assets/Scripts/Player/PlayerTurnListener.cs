using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerTurnListener : MonoBehaviour
    {
        private RotationOberver rotationOberver;
            
        public void Subscribe(RotationOberver rotationOberver)
        {
            this.rotationOberver = rotationOberver;
            rotationOberver.ChangedYRotation += Rotate;
            rotationOberver.ChangedPosition += ChangePosition;
        }

        public void Unsubscribe()
        {
            rotationOberver.ChangedYRotation -= Rotate;
            rotationOberver.ChangedPosition += ChangePosition;
        }

        private void ChangePosition(Vector3 delta)
        {
            transform.position = new Vector3(transform.position.x - delta.x, transform.position.y, transform.position.z + delta.z);
        }

        private void Rotate(float deltaY)
        {
            transform.Rotate(Vector3.up, deltaY);
        }
    }
}