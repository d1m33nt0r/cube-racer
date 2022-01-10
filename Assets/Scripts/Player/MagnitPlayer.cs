using System.Collections;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class MagnitPlayer : MonoBehaviour
{
    [SerializeField] private BoxCollider magnitCollider;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private GameObject magnit;

    private Transform startMarker;
    public Transform endMarker;

    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;

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
            MoveBoxIntoPlayer(other.transform);
        }
    }

    private void MoveBoxIntoPlayer(Transform box)
    {
        startMarker = box;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        StartCoroutine(Move());
        //var movePosition = transform.parent.parent.GetComponentInChildren<BoxController>().GetBoxPositionXYZ();
        //box.DOMove(new Vector3(movePosition.x, box.position.y, movePosition.z), 0);
    }

    private IEnumerator Move()
    {
        while (startMarker.position != endMarker.position)
        {
            if (startMarker.childCount == 0) break;
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            startMarker.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            yield return null;
        }
    }
}
