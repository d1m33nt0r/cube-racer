using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Boxes;
using UnityEngine;

public class MagnitPlayer : MonoBehaviour
{
    [SerializeField] private BoxCollider magnitCollider;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private GameObject magnit;

    public void EnableMagnitPlayerAndDestroyMagnitOnMap(GameObject gameObject)
    {
        Destroy(gameObject);
        magnitCollider.enabled = true;
        effect.Play();
        magnit.SetActive(true);
        StartCoroutine(DisableMagnit());
        transform.parent.GetComponent<BonusEffect>().Play();
    }

    private IEnumerator DisableMagnit()
    {
        yield return new WaitForSeconds(3.5f);
        effect.Stop();
        effect.Clear();
        magnit.SetActive(false);
        magnitCollider.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoxGroup"))
        {
            other.GetComponent<BoxGroup>().MoveToTargetTransform(transform);
        }
    }
}
