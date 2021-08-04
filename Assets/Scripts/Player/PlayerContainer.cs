using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerContainer : MonoBehaviour
    {
        [SerializeField] private Transform directionalLight;

        private float previousYRotation;

        private void Update()
        {
            if (previousYRotation == transform.rotation.y)
                return;
            
            if (TurnState.state == TurnState.State.Left)
            {
                directionalLight.rotation = Quaternion.Euler(directionalLight.transform.eulerAngles.x, 
                    directionalLight.transform.eulerAngles.y - Mathf.Abs(transform.eulerAngles.y - previousYRotation), 
                    directionalLight.transform.eulerAngles.z);
            }

            previousYRotation = transform.eulerAngles.y;
        }
        
    }
}