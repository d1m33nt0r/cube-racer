using DefaultNamespace;
using Diamond;
using Services.DataManipulator;
using UI;
using UI.GlobalUI.DiamondCounter;
using UnityEngine;
using Zenject;

public class DiamondCollectingEffect : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    [SerializeField] private Camera camera;
    [SerializeField] private UIController uiController;
    
    private AudioSource audioSource;
    private DiamondCounter diamondCounter;
    private DiamondUI diamondUI;
    private DiamondAudioController diamondsAudioController;
    private SessionDiamondCounter sessionDiamondCounter;
    private Vibrator vibrator;
    private DiamondMultiplierLevelManager m_diamondMultiplierLevelManager;

    [Inject]
    private void Construct(DiamondCounter diamondCounter, DiamondUI diamondUI,
        DiamondAudioController diamondsAudioController, SessionDiamondCounter sessionDiamondCounter,
        UIController uiController, Vibrator _vibrator, DiamondMultiplierLevelManager _diamondMultiplierLevelManager)
    {
        this.diamondCounter = diamondCounter;
        this.diamondUI = diamondUI;
        this.diamondsAudioController = diamondsAudioController;
        this.sessionDiamondCounter = sessionDiamondCounter;
        vibrator = _vibrator;
        m_diamondMultiplierLevelManager = _diamondMultiplierLevelManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.CompareTag("DiamondBonus") && other.CompareTag("DiamondCollector"))
        {
            uiController.ShowDimondBonusUIEffect();
            diamondsAudioController.Play();
            diamondCounter.AddDiamond(1500);
            sessionDiamondCounter.AddDiamond(1500);
            Destroy(gameObject);
        }

        if (!transform.CompareTag("DiamondBonus") && other.CompareTag("DiamondCollector"))
        {
            diamondsAudioController.Play();
            diamondCounter.AddDiamond();
            sessionDiamondCounter.AddDiamond(m_diamondMultiplierLevelManager.GetData());
            diamondUI.CreateDiamond(camera.WorldToScreenPoint(transform.position));
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(effect).transform.position = transform.position;
            vibrator.VibrateDiamond();
            Destroy(gameObject);
        }
    }
}