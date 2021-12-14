using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Services.AdsManager;
using PathCreation.Examples;
using UnityEngine;
using Zenject;

public class Zapravka : MonoBehaviour
{
    [SerializeField] private GameObject pathFollower;

    [SerializeField] private BoxCollider startBoxCollider;
    [SerializeField] private BoxCollider finishBoxCollider;

    private bool lalala;
    private AdsManager adsManager;
    private bool finish;
    private PlayerMover playerMover;

    [Inject]
    private void Construct(PlayerMover playerMover, AdsManager _adsManager)
    {
        this.playerMover = playerMover;
        adsManager = _adsManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (!lalala && other.CompareTag("DiamondCollector"))
        {
            Time.timeScale = 0;
            RewardedAds.Show();
            
            RewardedAds.rewardedAd.OnAdClosed += (_sender, _args) =>
            {
                Time.timeScale = 1;
                lalala = true;
                RewardedAds.Initialize();
            };
        }*/
        
        if (other.CompareTag("DiamondCollector") && !finish)
        {
            //Time.timeScale = 0;
            
            
            playerMover.DisableMoving();
            playerMover.UnsubscribeSwipes();
            //playerMover.DisablePhysics();
            pathFollower.GetComponent<PathFollower>().Moving += playerMover.CustomMove;
            pathFollower.GetComponent<PathFollower>().enabled = true;
            finishBoxCollider.enabled = true;
            StartCoroutine(Finish());
        }
 
        
        if (other.CompareTag("DiamondCollector") && finish)
        {
            playerMover.EnablePhysics();
            pathFollower.GetComponent<PathFollower>().Moving -= playerMover.CustomMove;
            playerMover.SetCurrentDirection();
            playerMover.EnableMoving();
            playerMover.SubscribeSwipes();
            pathFollower.GetComponent<PathFollower>().enabled = false;
        }
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(1);

        finish = true;
    }
}
