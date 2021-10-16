using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerContainer : MonoBehaviour
    {
        [SerializeField] private Transform directionalLight;
        [SerializeField] private Transform camera;
        
        private float previousYRotation;
        private bool rotating;
        
        private void Update()
        {
            if (previousYRotation == transform.rotation.y)
                return;

            if (!rotating)
                return;
            
            if (TurnState.state == TurnState.State.Left && TurnState.prevState == TurnState.State.Forward)
            {
                directionalLight.rotation = Quaternion.Euler(directionalLight.transform.eulerAngles.x, 
                    directionalLight.transform.eulerAngles.y - Mathf.Abs(transform.eulerAngles.y - previousYRotation), 
                    directionalLight.transform.eulerAngles.z);
            }
            
            if (TurnState.state == TurnState.State.Forward && TurnState.prevState == TurnState.State.Left)
            {
                directionalLight.rotation = Quaternion.Euler(directionalLight.transform.eulerAngles.x, 
                    directionalLight.transform.eulerAngles.y - Mathf.Abs(transform.eulerAngles.y - previousYRotation), 
                    directionalLight.transform.eulerAngles.z);
            }

            previousYRotation = transform.eulerAngles.y;
        }

        public void LightParent()
        {
            directionalLight.SetParent(transform);
        }

        public void CameraParent()
        {
            camera.SetParent(transform);
        }

        public void LightUnParent()
        {
            directionalLight.SetParent(null);
        }
        
        public void CameraUnParent()
        {
            camera.SetParent(null);
        }
        
        public void SetRotation(bool rotating)
        {
            this.rotating = rotating;
        }
        
    }
}