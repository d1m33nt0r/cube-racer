using DefaultNamespace;
using DefaultNamespace.General;
using DefaultNamespace.Services.AudioManager;
using DefaultNamespace.ThemeManager;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class FriendlyBox : MonoBehaviour
{
    [SerializeField] private bool isBoxBonus;
    [SerializeField] private GameObject lavaEffect;
    [SerializeField] private GameObject wowsomeCanvas;
    [SerializeField] private ParticleSystem wowsoneParticle;

    private bool used;
    private BoxController boxController;
    private AudioManager m_audioManager;
    private ThemeManager themeManager;
    private Vibrator vibrator;
    
    [Inject]
    public void Construct(BoxController boxController, AudioManager _audioManager, 
        ThemeManager themeManager, GameController gameController, Vibrator _vibrator)
    {
        this.boxController = boxController;
        m_audioManager = _audioManager;
        this.themeManager = themeManager;
        GetCurrentMaterial();
        vibrator = _vibrator;
    }

    public void Initialize(BoxController boxController, AudioManager _audioManager, 
        ThemeManager themeManager, GameController gameController, Vibrator _vibrator)
    {
        this.boxController = boxController;
        m_audioManager = _audioManager;
        this.themeManager = themeManager;
        GetCurrentMaterial();
        vibrator = _vibrator;
    }
    
    private void Awake()
    {
        GetCurrentMaterial();
    }

    private void GetCurrentMaterial()
    {
        var renderer = GetComponent<MeshRenderer>();
        if (themeManager == null) return;
        var material = themeManager.GetCurrentBoxTheme();
        renderer.sharedMaterial = material;
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
                m_audioManager.boxesAudioSource.PlayFailSound();
            }

            if (other.collider.CompareTag("Lava"))
            {
                Instantiate(lavaEffect, transform.position, Quaternion.identity);
                boxController.RemoveBox(gameObject, false, 1, true);
                m_audioManager.boxesAudioSource.PlayFailSound();
                vibrator.VibrateLava();
            }
            
            if (other.collider.CompareTag("Hole"))
            {
                //Instantiate(lavaEffect, transform.position, Quaternion.identity);
                boxController.RemoveBox(gameObject, false, 1, true);
                m_audioManager.boxesAudioSource.PlayFailSound();
                vibrator.VibrateLava();
            }
            
            if(other == null || other.collider == null || transform == null || other.collider.GetComponent<CenterPointGetter>() == null) return;
            
            if (other.collider.CompareTag("LevelFinish") && 
                Mathf.Abs( transform.TransformPoint(other.collider.GetComponent<CenterPointGetter>()
                    .GetCenterPoint()).y - transform.TransformPoint(transform.position).y) < 0.1f)
            {
                other.collider.tag = "Ground";
                boxController.RemoveBox(gameObject, true, 1);
                m_audioManager.boxesAudioSource.PlayFailSound();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBoxBonus) return;
        if (used) return;
        if (other.CompareTag("DiamondCollector"))
        {
            used = true;
            boxController.SpecialAddBox2(transform.parent.GetComponent<BoxesPool>().Boxes);
            wowsomeCanvas.SetActive(true);
            wowsoneParticle.Play();
            boxController.AnimateEmission();
            other.GetComponent<BonusEffect>().Play();
            Destroy(gameObject);
        }
    }
}