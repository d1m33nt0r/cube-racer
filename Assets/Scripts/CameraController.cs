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
    public float distanceZFromPlayer;
    
    public CamPoint(Vector3 pos, Vector3 rot, float fieldView, float zDistance)
    {
        position = pos;
        rotation = rot;
        this.fieldView = fieldView;
        distanceZFromPlayer = zDistance;
    }
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private int maxCount;
    [SerializeField] private int minCount;
    [SerializeField] private Camera camera;
    
    [SerializeField] private float yPositionValue = 0.75f;
    [SerializeField] private float xPositionValue = 0.75f;
    [SerializeField] private float effectDuration = 0.25f;
    [SerializeField] private float fieldViewValue = 0.75f;
    [SerializeField] private float zOffsetValue;

    [SerializeField] private float speedForChangingPositionOnRefuelling;
    
    private BoxController boxController;
    private float currentZValue;
    private Dictionary<int, CamPoint> camPoints = new Dictionary<int, CamPoint>();

    [Inject]
    private void Construct(BoxController boxController)
    {
        maxCount = 25;
        minCount = 4;
        this.boxController = boxController;
        boxController.AddedBoxes += Increase;
        boxController.SpecialAddedBox += SpecialIncrease;
        boxController.RemovedBox += Decrease;
        FillCameraPoints();
    }

    private void FillCameraPoints()
    {
        var cameraFieldOfView = GetComponent<Camera>().fieldOfView;
        var j = 0;
        for (var i = maxCount - (maxCount - minCount); i <= maxCount; i++)
        {
            var multiplier = i - (maxCount - (maxCount - minCount));
            
            var pos = new Vector3(transform.localPosition.x + xPositionValue * multiplier, transform.localPosition.y + yPositionValue * multiplier, 
                transform.localPosition.z - zOffsetValue * multiplier);
            
            var rot = new Vector3(transform.localRotation.eulerAngles.x, 
                transform.localRotation.eulerAngles.y + effectDuration * multiplier, transform.localRotation.eulerAngles.z);
            
            var fieldView = cameraFieldOfView + fieldViewValue * multiplier;

            var zDistance = transform.localPosition.z - zOffsetValue * multiplier;
            
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

    private void Increase(int count)
    {
        var pointIsExist = TryGetPoint<CamPoint>(boxController.boxCount, out var camPoint);

        if (!pointIsExist)
        {
            if (boxController.prevBoxCount < maxCount && boxController.boxCount > minCount)
            {
                var sad = TryGetPoint<CamPoint>(maxCount, out var camPoint2);
                if (sad)
                {
                    transform.DOKill();
                    camera.DOKill();
            
                    transform.DOLocalMove(camPoint2.position, effectDuration);
                    camera.DOFieldOfView(camPoint2.fieldView, effectDuration );
                }
            }
        }
        
        if (pointIsExist)
        {
            transform.DOKill();
            camera.DOKill();
            
            transform.DOLocalMove(camPoint.position, effectDuration);
            camera.DOFieldOfView(camPoint.fieldView, effectDuration );
        }
    }

    private void SpecialIncrease()
    {
        var pointIsExist = TryGetPoint<CamPoint>(boxController.boxCount, out var camPoint);

        if (!pointIsExist)
        {
            if (boxController.prevBoxCount < maxCount)
            {
                var sad = TryGetPoint<CamPoint>(maxCount, out var camPoint2);
                if (sad)
                {
                    transform.DOKill();
                    camera.DOKill();
                    
       
                    transform.DOLocalMove(camPoint2.position, 2f);
                    camera.DOFieldOfView(camPoint2.fieldView, 2f);
                }
            }
        }
        
        if (pointIsExist)
        {
            transform.DOKill();
            camera.DOKill();
            
            transform.DOLocalMove(camPoint.position, 2f);
            camera.DOFieldOfView(camPoint.fieldView, 2f);
        }
        
        ChangeFinishingPosition();
    }

    private void Decrease(bool finish, int multiplier)
    {
        if (finish)
        {
            camera.transform.DOMoveY(transform.position.y + 0.1f, 0.25f);
            return;
        }
        
        var pointIsExist = TryGetPoint<CamPoint>(boxController.boxCount, out var camPoint);

        if (!pointIsExist)
        {
            if (boxController.prevBoxCount > minCount && boxController.boxCount < minCount)
            {
                var sad = TryGetPoint<CamPoint>(minCount, out var camPoint2);
                if (sad)
                {
                    transform.DOKill();
                    camera.DOKill();
            
                    transform.DOLocalMove(camPoint2.position, effectDuration);
                    camera.DOFieldOfView(camPoint2.fieldView, effectDuration);
                }
            }
        }
        
        if (pointIsExist)
        {
            transform.DOKill();
            camera.DOKill();
            
            transform.DOLocalMove(camPoint.position, effectDuration);
            camera.DOFieldOfView(camPoint.fieldView, effectDuration);
        }
    }

    public void FinishRotation(Transform target)
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

    public void ChangeStartingPosition()
    {
        transform.DOLocalRotate(new Vector3(17.7f, -3f, 0), speedForChangingPositionOnRefuelling * 0.7f);
        transform.DOLocalMoveX(0, speedForChangingPositionOnRefuelling * 0.7f);
        //transform.DOLocalMoveZ(-33.6f, speedForChangingPositionOnRefuelling * 0.7f);
    }

    private void ChangeFinishingPosition()
    {
        transform.DOLocalRotate(new Vector3(17.7f, -11.54f, 0), speedForChangingPositionOnRefuelling);
        transform.DOLocalMoveX(8.4f, speedForChangingPositionOnRefuelling);
        //transform.DOLocalMoveZ(-27.91f, speedForChangingPositionOnRefuelling);
    }
}