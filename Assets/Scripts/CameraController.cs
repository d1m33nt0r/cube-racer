using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CamPoint
{
    public Vector3 position;
    public Vector3 rotation;
    public float fieldView;

    public CamPoint(Vector3 pos, Vector3 rot, float fieldView)
    {
        position = pos;
        rotation = rot;
        this.fieldView = fieldView;
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
    [SerializeField] private float zOffsetDuration;
    
    private BoxController boxController;
    
    private Vector3 startRotation;
    private float startYPosition;

    private Dictionary<int, CamPoint> camPoints = new Dictionary<int, CamPoint>();

    [Inject]
    private void Construct(BoxController boxController)
    {
        maxCount = 25;
        minCount = 4;
        this.boxController = boxController;
        boxController.AddedBoxes += IncreaseY;
        boxController.RemovedBox += DecreaseY;
        startRotation = transform.localRotation.eulerAngles;
        startYPosition = transform.position.y;
        FillCameraPoints();
    }

    private void FillCameraPoints()
    {
        for (var i = maxCount - (maxCount - minCount); i < maxCount; i++)
        {
            var multiplier = i - (maxCount - (maxCount - minCount));
            var pos = new Vector3(transform.position.x, transform.position.y + yPositionValue * multiplier, transform.position.z);
            var rot = new Vector3(transform.localRotation.eulerAngles.x + xRotateValue * multiplier, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
            var fieldView = GetComponent<Camera>().fieldOfView + fieldViewValue * multiplier;
            camPoints.Add(i, new CamPoint(pos, rot, fieldView));
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

    private void IncreaseY(int countBoxes)
    {
        var pointIsExist = TryGetPoint<CamPoint>(boxController.boxCount, out var camPoint);
        
        if (pointIsExist)
        {
            transform.DOMoveY(camPoint.position.y, verticalMoveDuration);
            camera.transform.DOLocalRotate(
                new Vector3(camPoint.rotation.x, transform.localRotation.eulerAngles.y,
                    transform.localRotation.eulerAngles.z), rotationDuration);
            camera.DOFieldOfView(camPoint.fieldView, fieldViewDuration);
        }
    }

    private void DecreaseY(bool finish, int multiplier)
    {
        var pointIsExist = TryGetPoint<CamPoint>(boxController.boxCount, out var camPoint);
        
        if (pointIsExist)
        {
            camera.transform.DOMoveY(camPoint.position.y, verticalMoveDuration);
            camera.transform.DOLocalRotate(
                new Vector3(camPoint.rotation.x, transform.localRotation.eulerAngles.y,
                    transform.localRotation.eulerAngles.z), rotationDuration);
            camera.DOFieldOfView(camPoint.fieldView, fieldViewDuration);
        }
        


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