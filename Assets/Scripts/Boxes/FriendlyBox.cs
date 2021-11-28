using DefaultNamespace;
using DefaultNamespace.ThemeManager;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class FriendlyBox : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject plusOne;
    [SerializeField] private bool isBoxBonus;
    [SerializeField] private GameObject boxes;
    [SerializeField] private GameObject lavaEffect;
    [SerializeField] private GameObject wowsomeCanvas;
    [SerializeField] private ParticleSystem wowsoneParticle;
    [SerializeField] private Light _light;
    
    private BoxController boxController;
    private BoxAudioController boxAudioController;
    private ThemeManager themeManager;
    private Vibrator vibrator;

    [Inject] 
    public void Construct(BoxController boxController, BoxAudioController boxAudioController, 
        ThemeManager themeManager, GameController gameController, Vibrator _vibrator)
    {
        this.boxController = boxController;
        this.boxAudioController = boxAudioController;
        this.themeManager = themeManager;
        vibrator = _vibrator;
    }

    private void Start()
    {
        GetCurrentMaterial();
    }

    private void GetCurrentMaterial()
    {
        GetComponent<MeshRenderer>().sharedMaterial = themeManager.GetCurrentBoxTheme();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isBoxBonus)
        {
            if (!transform.parent || transform.parent.name != "Player")
                return;

            if (other.collider.CompareTag("LetBox") && Mathf.Abs(other.transform.position.y - transform.position.y) < 0.1f)
            {
                boxController.RemoveBox(gameObject, false, 1);
                boxAudioController.PlayFailSound();
            }

            if (other.collider.CompareTag("Hole"))
            {
                Instantiate(lavaEffect, transform.position, Quaternion.identity);
                boxController.RemoveBox(gameObject, false, 1, true);
                boxAudioController.PlayFailSound();
                vibrator.VibrateLava();
            }
            
            if(other == null || other.collider == null || transform == null || other.collider.GetComponent<CenterPointGetter>() == null) return;
            
            if (other.collider.CompareTag("LevelFinish") && 
                Mathf.Abs( transform.TransformPoint(other.collider.GetComponent<CenterPointGetter>()
                    .GetCenterPoint()).y - transform.TransformPoint(transform.position).y) < 0.1f)
            {
                other.collider.tag = "Ground";
                boxController.RemoveBox(gameObject, true, 1);
                boxAudioController.PlayFailSound();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBoxBonus) return;
        
        if (other.CompareTag("DiamondCollector"))
        {
            boxController.SpecialAddBox(5);
            wowsomeCanvas.SetActive(true);
            wowsoneParticle.Play();
            
            /*_light.enabled = true;
            _light.DOIntensity(4f, 0.05f).onComplete = () =>
            {
                _light.DOIntensity(0, 0.25f).onComplete = () =>
                {
                    _light.enabled = false;
                };
            };*/
            
            Destroy(gameObject);
        }
    }

    private void SpawnEffects()
    {
        var effectPosition = new Vector3(transform.position.x,
            transform.position.y , transform.position.z);

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