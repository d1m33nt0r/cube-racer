using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private int maxCount;
    [SerializeField] private int startValue;
    [SerializeField] private Camera camera;

    [SerializeField] private float yPositionValue = 0.75f;
    [SerializeField] private float verticalMoveDuration = 0.25f;
    [SerializeField] private float xRotateValue = 1;
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float fieldViewValue = 0.75f;
    [SerializeField] private float fieldViewDuration = 0.25f;
    [SerializeField] private float zOffsetValue;
    [SerializeField] private float zOffsetDuration;
    
    private int previousCount;
    private BoxController boxController;
    private Coroutine coroutine;

    private Vector3 startRotation;
    private float startYPosition;
    
    [Inject]
    private void Construct(BoxController boxController)
    {
        this.boxController = boxController;
        previousCount = boxController.boxCount;
        boxController.AddedBox += IncreaseY;
        boxController.RemovedBox += DecreaseY;
        startRotation = transform.localRotation.eulerAngles;
        startYPosition = transform.localPosition.y;
    }
    
    private void IncreaseY()
    {
        var difference = boxController.boxCount - previousCount;
        if (maxCount >= boxController.boxCount && boxController.boxCount > startValue)
        {
            camera.transform.DOMoveY(transform.position.y + yPositionValue, verticalMoveDuration);
            camera.transform.DOLocalRotate(
                new Vector3(transform.localRotation.eulerAngles.x + xRotateValue, transform.localRotation.eulerAngles.y,
                    transform.localRotation.eulerAngles.z), rotationDuration);
            camera.DOFieldOfView(camera.fieldOfView + fieldViewValue, fieldViewDuration);
        }
        previousCount = boxController.boxCount;
    }

    private void DecreaseY(bool finish, int multiplier)
    {
        var difference = boxController.boxCount - previousCount;
        
        if (startValue < boxController.boxCount && !finish)
        {
            camera.transform.DOMoveY(transform.position.y - yPositionValue, verticalMoveDuration);
            camera.transform.DOLocalRotate(
                new Vector3(transform.localRotation.eulerAngles.x - xRotateValue, transform.localRotation.eulerAngles.y,
                    transform.localRotation.eulerAngles.z), rotationDuration);
            camera.DOFieldOfView(camera.fieldOfView - fieldViewValue, fieldViewDuration);
        }

        if (startValue >= boxController.boxCount && !finish)
        {
            transform.DOLocalMoveY(startYPosition, 0.25f);
            camera.transform.DOLocalRotate(startRotation, 0.5f);
            camera.DOFieldOfView(60, 0.25f);
        }
        
        previousCount = boxController.boxCount;
        
        if (finish)
            camera.transform.DOMoveY(transform.position.y + 0.15f, 0.25f);
    }

    public void RotateAround(Transform target)
    {
        StartCoroutine(Rotation(target));
    }

    private IEnumerator Rotation(Transform target)
    {
        while (true)
        {
            transform.RotateAround(target.position, Vector3.up, 20 * Time.deltaTime);
            yield return null;
        }
    }
}