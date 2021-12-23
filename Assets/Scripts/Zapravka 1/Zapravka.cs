using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
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
   
        
        if (other.CompareTag("DiamondCollector") && !finish)
        {
            //Time.timeScale = 0;
            
            adsManager.ShowRewarded();
            
            RewardedAds.rewardedAd.OnAdClosed += (object _sender, EventArgs _e) =>
            {
                playerMover.DisableMoving();
                playerMover.UnsubscribeSwipes();
                //playerMover.DisablePhysics();
                pathFollower.GetComponent<PathFollower>().Moving += playerMover.CustomMove;
                pathFollower.GetComponent<PathFollower>().enabled = true;
                finishBoxCollider.enabled = true;
                StartCoroutine(Finish());
            };
        }
 
        
        if (other.CompareTag("DiamondCollector") && finish)
        {
          
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
