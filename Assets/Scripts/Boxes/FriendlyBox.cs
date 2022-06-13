using System.Collections;
using DefaultNamespace;
using DefaultNamespace.General;
using DefaultNamespace.Services.AudioManager;
using DefaultNamespace.ThemeManager;
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

    private const string LET_BOX = "LetBox";
    private const string GROUND_LET = "GroundLet";
    private const string LAVA = "Lava";
    private const string HOLE = "Hole";
    private void OnCollisionEnter(Collision other)
    {
        if (!isBoxBonus)
        {
            if (!transform.parent || transform.parent.name != PLAYER)
                return;

            if (other.collider.CompareTag(LET_BOX) && Mathf.Abs(other.transform.position.y - transform.position.y) < 0.1f)
            {
                boxController.RemoveBox(gameObject, false, 1);
                m_audioManager.boxesAudioSource.PlayFailSound();
            }
            
            if (other.collider.CompareTag(GROUND_LET))
            {
                boxController.RemoveBox(gameObject, false, 1);
                m_audioManager.boxesAudioSource.PlayFailSound();
            }

            if (other.collider.CompareTag(LAVA))
            {
                Instantiate(lavaEffect, transform.position, Quaternion.identity);
                boxController.RemoveBox(gameObject, false, 1, true);
                m_audioManager.boxesAudioSource.PlayFailSound();
#if UNITY_ANDROID
                vibrator.VibrateLava();
#endif
            }
            
            if (other.collider.CompareTag(HOLE))
            {
                //Instantiate(lavaEffect, transform.position, Quaternion.identity);
                boxController.RemoveBox(gameObject, false, 1, true);
                m_audioManager.boxesAudioSource.PlayFailSound();
#if UNITY_ANDROID
                vibrator.VibrateLava();
#endif
            }
            
            if(other == null || other.collider == null || transform == null || other.collider.GetComponent<CenterPointGetter>() == null) return;
            
            if (other.collider.CompareTag(LEVEL_FINISH) && 
                Mathf.Abs( transform.TransformPoint(other.collider.GetComponent<CenterPointGetter>()
                    .GetCenterPoint()).y - transform.TransformPoint(transform.position).y) < 0.1f)
            {
                other.collider.tag = GROUND;
                boxController.RemoveBox(gameObject, true, 1);
                m_audioManager.boxesAudioSource.PlayFailSound();
            }
        }
    }

    private const string LEVEL_FINISH = "LevelFinish";
    private const string GROUND = "Ground";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(FINISH_CAMERA_CONFIGURATOR) && transform.parent.name == PLAYER)
        {
            Camera.main.GetComponent<CameraController>().ConfigureCameraForFinish();
        }
        
        if (!isBoxBonus) return;
        if (used) return;
        if (other.CompareTag(DIAMOND_COLLECTOR))
        {
            used = true;
            boxController.SpecialAddBox2(transform.parent.GetComponent<BoxesPool>().Boxes);
            Camera.main.GetComponent<CameraController>().ChangeFinishingPosition();
            //wowsomeCanvas.SetActive(true);
            //wowsoneParticle.Play();
            boxController.AnimateEmission();
            other.GetComponent<BonusEffect>().Play();
            Destroy(gameObject);
        }
    }

    private const string DIAMOND_COLLECTOR = "DiamondCollector";
    private const string PLAYER = "Player";
    private const string FINISH_CAMERA_CONFIGURATOR = "FinishCameraConfigurator";
}