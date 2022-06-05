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
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1);
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

    private void ReceivedReward(string s, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        ReleaseReward();
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= ReceivedReward;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= DisplayFailedEvent;
    }

    private void DisplayFailedEvent(string s, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        ReleaseReward();
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= ReceivedReward;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= DisplayFailedEvent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(DIAMOND_COLLECTOR_TAG) && !finish)
        {
            if (rewardShowed) return;
            if (adsManager.RewardedAd.IsRewardedAdAlready)
            {
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += ReceivedReward;
                MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += DisplayFailedEvent;
                adsManager.ShowRewarded();
                
                rewardShowed = true;
            }
            else
            {
                ReleaseReward();
            }
        }
        
        if (other.CompareTag(DIAMOND_COLLECTOR_TAG) && finish)
        {
            pathFollower.Moving -= playerMover.CustomMove;
            playerMover.SetCurrentDirection();
            playerMover.EnableMoving();
            playerMover.SubscribeSwipes();
            pathFollower.enabled = false;
        }
    }

    private void ReleaseReward() 
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
        yield return waitForSeconds;

        finish = true;
    }
}
