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
        if (other.CompareTag("DiamondCollector") && !finish)
        {
            if (rewardShowed) return;
            adsManager.ShowInterstitial();
            rewardShowed = true;
            InterstitialAds.InterstitialAd.OnAdClosed += ReleaseReward;
            InterstitialAds.InterstitialAd.OnAdFailedToShow += ReleaseReward;
        }
        
        if (other.CompareTag("DiamondCollector") && finish)
        {
            pathFollower.Moving -= playerMover.CustomMove;
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
        BannerAds.Show();
    }
    
    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(1);

        finish = true;
    }
}
