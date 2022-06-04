using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Services.AdsManager;
using Firebase.Analytics;
using PathCreation.Examples;
using UnityEngine;
using Zenject;

public class Zapravka : MonoBehaviour
{
    [SerializeField] private PathFollower pathFollower;

    [SerializeField] private BoxCollider startBoxCollider;
    [SerializeField] private BoxCollider finishBoxCollider;
    
    private const string DIAMOND_COLLECTOR_TAG = "DiamondCollector";
    
    private AdsManager adsManager;
    private bool finish;
    private PlayerMover playerMover;
    private bool rewardShowed;
    
    [Inject]
    private void Construct(PlayerMover playerMover, AdsManager _adsManager)
    {
        this.playerMover = playerMover;
        adsManager = _adsManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(DIAMOND_COLLECTOR_TAG) && !finish)
        {
            if (rewardShowed) return;
            adsManager.ShowRewarded();
            rewardShowed = true;
            RewardedAds.rewardedAd.OnAdClosed += ReleaseReward;
            RewardedAds.rewardedAd.OnAdFailedToShow += ReleaseReward;
        }
        
        if (other.CompareTag(DIAMOND_COLLECTOR_TAG) && finish)
        {
            pathFollower.Moving -= playerMover.CustomMove;
            RewardedAds.rewardedAd.OnAdClosed -= ReleaseReward;
            RewardedAds.rewardedAd.OnAdFailedToShow -= ReleaseReward;
            playerMover.SetCurrentDirection();
            playerMover.EnableMoving();
            playerMover.SubscribeSwipes();
            pathFollower.enabled = false;
        }
    }

    private void ReleaseReward(object _sender, EventArgs _e) 
    {
        playerMover.DisableMoving();
        playerMover.UnsubscribeSwipes();
        pathFollower.Moving += playerMover.CustomMove;
        var cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SaveCurrentPositionAndRotation();
        cameraController.ChangeStartingPosition();
        pathFollower.enabled = true;
        finishBoxCollider.enabled = true;
        StartCoroutine(Finish());
    }
    
    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(1);

        finish = true;
    }
}
