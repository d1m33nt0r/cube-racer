using System.Collections;
using DG.Tweening;
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
    }

    private IEnumerator DisableMagnit()
    {
        yield return new WaitForSeconds(5);
        effect.Stop();
        effect.Clear();
        magnit.SetActive(false);
        magnitCollider.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoxGroup"))
        {
            MoveBoxIntoPlayer(other.transform);
        }
    }

    private void MoveBoxIntoPlayer(Transform box)
    {
        var movePosition = transform.parent.parent.GetComponentInChildren<BoxController>().GetBoxPositionXZ();
        box.DOMove(new Vector3(movePosition.x, box.position.y, movePosition.z), 0.1f);
    }
}
