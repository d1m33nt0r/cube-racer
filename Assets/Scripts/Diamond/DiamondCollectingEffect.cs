using Diamond;
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

    
    [Inject]
    private void Construct(DiamondCounter diamondCounter, DiamondUI diamondUI, 
        DiamondAudioController diamondsAudioController, SessionDiamondCounter sessionDiamondCounter,
        UIController uiController)
    {
        this.diamondCounter = diamondCounter;
        this.diamondUI = diamondUI;
        this.diamondsAudioController = diamondsAudioController;
        this.sessionDiamondCounter = sessionDiamondCounter;
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
            sessionDiamondCounter.AddDiamond();
            diamondUI.CreateDiamond(camera.WorldToScreenPoint(transform.position));
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(effect).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}