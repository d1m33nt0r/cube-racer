using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class CheckInternetConnection : MonoBehaviour
    {
        private Animator animator;
        private Canvas canvas;
        
        private void Start()
        {
            animator = GetComponent<Animator>();
            canvas = GetComponent<Canvas>();
            DisableCanvas();
            CheckInternet();
        }

        public void CheckInternet()
        {
            StartCoroutine(CheckConnection());
        }

        private IEnumerator CheckConnection()
        {
            var internetAccess = Application.internetReachability != NetworkReachability.NotReachable;
            var request = new UnityWebRequest("https://google.com");
            yield return request.SendWebRequest();

            if (request.error != null || !internetAccess)
            {
                EnableCanvas();
                animator.Play("Show");
            }
            else
            {
                DisableCanvas();
            }
        }

        public void Hide()
        {
            animator.Play("Hide");
        }
        
        private void DisableCanvas()
        {
            canvas.enabled = false;
        }

        private void EnableCanvas()
        {
            canvas.enabled = true;
        }
    }
}