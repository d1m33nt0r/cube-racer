using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using Zenject;

public class CamPoint
{
    public Vector3 position;
    public Vector3 rotation;
    public float fieldView;
    public float distanceZFromPlayer;
    
    public CamPoint(Vector3 pos, Vector3 rot, float fieldView, float zDistance)
    {
        position = pos;
        rotation = rot;
        this.fieldView = fieldView;
        this.distanceZFromPlayer = zDistance;
    }
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private int maxCount;
    [SerializeField] private int minCount;
    [SerializeField] private Camera camera;
    
    [SerializeField] private float yPositionValue = 0.75f;
    [SerializeField] private float verticalMoveDuration = 0.25f;
    [SerializeField] private float xRotateValue = 0.1f;
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float fieldViewValue = 0.75f;
    [SerializeField] private float fieldViewDuration = 0.25f;
    [SerializeField] private float zOffsetValue;
    [SerializeField] private float zMoveDuration;
    
    private BoxController boxController;

    private float currentZValue;
    
    private Vector3 startRotation;
    private float startYPosition;

    private Dictionary<int, CamPoint> camPoints = new Dictionary<int, CamPoint>();

    [Inject]
    private void Construct(BoxController boxController)
    {
        maxCount = 30;
        minCount = 4;
        this.boxController = boxController;
        boxController.AddedBoxes += Increase;
        boxController.SpecialAddedBox += SpecialIncrease;
        boxController.RemovedBox += Decrease;
        startRotation = transform.localRotation.eulerAngles;
        startYPosition = transform.position.y;
        FillCameraPoints();
    }

    private void FillCameraPoints()
    {
        var j = 0;
        for (var i = maxCount - (maxCount - minCount); i < maxCount; i++)
        {
            var multiplier = i - (maxCount - (maxCount - minCount));
            
            var pos = new Vector3(transform.position.x, transform.position.y + yPositionValue * multiplier, 
                transform.position.z - zOffsetValue * multiplier);
            
            var rot = new Vector3(transform.localRotation.eulerAngles.x + xRotateValue * multiplier, 
                transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
            
            var fieldView = GetComponent<Camera>().fieldOfView + fieldViewValue * multiplier;

            var zDistance = Mathf.Abs(boxController.LocalPosition.z - transform.localPosition.z) + boxController.LocalPosition.z - zOffsetValue * multiplier;
            
            camPoints.Add(i, new CamPoint(pos, rot, fieldView, zDistance));
        }
    }

    private bool TryGetPoint<T>(int countBoxes, out CamPoint value)
    {
        if (camPoints.ContainsKey(countBoxes))
        {
            value = camPoints[countBoxes];
            return true;
        }

        value = null;
        return false;
    }

    private void Increase()
    {
        var pointIsExist = TryGetPoint<CamPoint>(boxController.boxCount, out var camPoint);
        
        if (pointIsExist)
        {
            transform.DOKill();
            camera.DOKill();
            
            transform.DOMoveY(camPoint.position.y, verticalMoveDuration * (boxController.boxCount - boxController.prevBoxCount));
            IncreaseZ(camPoint);
            transform.DOLocalRotate(
                new Vector3(camPoint.rotation.x, transform.localRotation.eulerAngles.y,
                    transform.localRotation.eulerAngles.z), rotationDuration * (boxController.boxCount - boxController.prevBoxCount));
            camera.DOFieldOfView(camPoint.fieldView, fieldViewDuration * (boxController.boxCount - boxController.prevBoxCount));
        }
    }

    private void IncreaseZ(CamPoint _camPoint)
    {
        var actualDistance = -Mathf.Abs(boxController.transform.localPosition.z - transform.localPosition.z);
        var difference = Mathf.Abs(_camPoint.distanceZFromPlayer - actualDistance);
        transform.DOLocalMoveZ(transform.localPosition.z - difference, zMoveDuration);
    }
    
    private void DecreaseZ(CamPoint _camPoint)
    {
        var actualDistance = -Mathf.Abs(boxController.transform.localPosition.z - transform.localPosition.z);
        var difference = Mathf.Abs(_camPoint.distanceZFromPlayer - actualDistance);
        transform.DOLocalMoveZ(transform.localPosition.z + difference, zMoveDuration);
    }
    
    private void SpecialIncrease()
    {
        var pointIsExist = TryGetPoint<CamPoint>(boxController.boxCount, out var camPoint);

        if (pointIsExist)
        {
            transform.DOKill();
            camera.DOKill();
            
            transform.DOMoveY(camPoint.position.y, 1.5f);
            
            IncreaseZ(camPoint);
            transform.DOLocalRotate(
                new Vector3(camPoint.rotation.x, transform.localRotation.eulerAngles.y,
                    transform.localRotation.eulerAngles.z), 1.5f);
            camera.DOFieldOfView(camPoint.fieldView, 1.5f);
        }
    }

    private void Decrease(bool finish, int multiplier)
    {
        if (finish)
        {
            camera.transform.DOMoveY(transform.position.y + 0.15f, 0.25f);
            return;
        }
        
        var pointIsExist = TryGetPoint<CamPoint>(boxController.boxCount, out var camPoint);
        
        if (pointIsExist)
        {
            transform.DOKill();
            camera.DOKill();
            
            transform.DOMoveY(camPoint.position.y, verticalMoveDuration * 2);
            DecreaseZ(camPoint);
            transform.DOLocalRotate(
                new Vector3(camPoint.rotation.x, transform.localRotation.eulerAngles.y,
                    transform.localRotation.eulerAngles.z), rotationDuration  * 2);
            camera.DOFieldOfView(camPoint.fieldView, fieldViewDuration  * 2);
        }
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