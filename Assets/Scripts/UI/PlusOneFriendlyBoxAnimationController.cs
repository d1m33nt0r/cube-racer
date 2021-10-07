using UnityEngine;

namespace UI
{
    public class PlusOneFriendlyBoxAnimationController : MonoBehaviour
    {
        [SerializeField] private GameObject plusOne;
        [SerializeField] private GameObject effect;
        
        public void SpawnEffect(Vector3 effectPosition)
        {
            var effect2 = Instantiate(effect);
            effect2.transform.position = effectPosition;
            effect2.transform.SetParent(transform);

            var effectText = Instantiate(plusOne);
            effectText.transform.position =
                new Vector3(effectPosition.x, effectPosition.y, effectPosition.z);
            effectText.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            effectText.transform.SetParent(transform);
        }
    }
}