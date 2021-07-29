using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private int maxCount;
    [SerializeField] private int startValue;
    [SerializeField] private Camera camera;

    private int previousCount;
    private BoxController boxController;
    private Coroutine coroutine;

    [Inject]
    private void Construct(BoxController boxController)
    {
        this.boxController = boxController;
        previousCount = boxController.boxCount;
        boxController.AddedBox += IncreaseY;
        boxController.RemovedBox += DecreaseY;
    }


    private void IncreaseY()
    {
        var difference = boxController.boxCount - previousCount;
        if (maxCount >= boxController.boxCount && boxController.boxCount > startValue)
        {
            camera.transform.DOMoveY(transform.position.y + 0.075f, 0.25f);
            camera.DOFieldOfView(camera.fieldOfView + 0.75f, 0.25f);
        }
        previousCount = boxController.boxCount;
    }

    private void DecreaseY(bool finish, int multiplier)
    {
        var difference = boxController.boxCount - previousCount;
        
        if (startValue < boxController.boxCount && !finish)
        {
            camera.transform.DOMoveY(transform.position.y - 0.075f, 0.25f);
            camera.DOFieldOfView(camera.fieldOfView - 0.75f, 0.25f);
        }

        if (startValue == boxController.boxCount && !finish)
        {
            transform.DOLocalMoveY(15.59f, 0.25f);
            camera.DOFieldOfView(57, 0.25f);
        }
        
        previousCount = boxController.boxCount;
        
        if (finish)
            camera.transform.DOMoveY(transform.position.y + 0.15f, 0.25f);
    }
}