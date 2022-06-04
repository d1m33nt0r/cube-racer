using DefaultNamespace;
using DefaultNamespace.ObjectPool;
using DefaultNamespace.Services.AudioManager;
using Diamond;
using Services.DataManipulator;
using UI;
using UI.GlobalUI.DiamondCounter;
using UnityEngine;
using Zenject;

public class DiamondCollectingEffect : MonoBehaviour
{
    private const string DIAMOND_COLLECTOR_TAG = "DiamondCollector";
    private const string DIAMOND_BONUS_TAG = "DiamondBonus";
    private const string DIAMOND_EFFECT = "diamond_effect";
    
    [SerializeField] private GameObject effect;
    [SerializeField] private Camera camera;
    [SerializeField] private UIController uiController;
    [SerializeField] private GameObject wowsomeCanvas;
    [SerializeField] private ParticleSystem wowsoneParticle;
    
    private AudioSource audioSource;
    private DiamondCounter diamondCounter;
    private DiamondUI diamondUI;
    private AudioManager m_audioManager;
    private SessionDiamondCounter sessionDiamondCounter;
    private Vibrator vibrator;
    private DiamondMultiplierLevelManager m_diamondMultiplierLevelManager;
    private PoolManager m_poolManager;
    
    [Inject]
    private void Construct(DiamondCounter diamondCounter, DiamondUI diamondUI,
        AudioManager _audioManager, SessionDiamondCounter sessionDiamondCounter,
        UIController uiController, Vibrator _vibrator, DiamondMultiplierLevelManager _diamondMultiplierLevelManager,
        PoolManager _poolManager)
    {
        this.diamondCounter = diamondCounter;
        this.diamondUI = diamondUI;
        this.m_audioManager = _audioManager;
        this.sessionDiamondCounter = sessionDiamondCounter;
        vibrator = _vibrator;
        m_diamondMultiplierLevelManager = _diamondMultiplierLevelManager;
        m_poolManager = _poolManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.CompareTag(DIAMOND_BONUS_TAG) && other.CompareTag(DIAMOND_COLLECTOR_TAG))
        {
            uiController.ShowDimondBonusUIEffect();
            m_audioManager.diamondAudioSource.PlayCollectSound();
            other.GetComponent<BonusEffect>().Play();
            other.transform.parent.GetComponent<BoxController>().AnimateEmission();
            wowsomeCanvas.SetActive(true);
            //wowsoneParticle.Play();
            diamondCounter.AddDiamond(750);
            sessionDiamondCounter.AddDiamond(750);
            Camera.main.GetComponent<CameraController>().ChangeFinishingPosition();
            Destroy(gameObject);
        }

        if (!transform.CompareTag(DIAMOND_BONUS_TAG) && other.CompareTag(DIAMOND_COLLECTOR_TAG))
        {
            m_audioManager.diamondAudioSource.PlayCollectSound();
            diamondCounter.AddDiamond();
            sessionDiamondCounter.AddDiamond(m_diamondMultiplierLevelManager.GetData());
            diamondUI.CreateDiamond(camera.WorldToScreenPoint(transform.position));
            GetComponent<BoxCollider>().enabled = false;
            //Instantiate(effect).transform.position = transform.position;
            var temp = m_poolManager.GetObject(DIAMOND_EFFECT);
            temp.transform.position = transform.position;
            temp.SetActive(true);
#if UNITY_ANDROID
            vibrator.VibrateDiamond();
#endif
            Destroy(gameObject);
        }
    }
}