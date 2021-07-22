using System.Collections;
using UnityEngine;
using Zenject;

public class FieldViewController : MonoBehaviour
{
    [SerializeField] private float bias;
    [SerializeField] private float distance;
    [SerializeField] private int maxCount;
    [SerializeField] private int startValue;
    [SerializeField] private Camera camera;

    private int previousCount;
    private BoxController boxController;
    private Coroutine coroutine;
    
    public float minimum = -1.0F;
    public float maximum =  1.0F;
    
    static float t = 0.0f;
    
    [Inject]
    private void Construct(BoxController boxController)
    {
        this.boxController = boxController;
        boxController.BoxCountChanged += ChangeFieldView;
    }

    private void ChangeFieldView()
    {
        if (previousCount < boxController.boxCount && maxCount >= boxController.boxCount && boxController.boxCount > startValue)
        {
            var difference = boxController.boxCount - previousCount;
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(UpdateFieldView(true, difference));
        }
        else if (previousCount > boxController.boxCount && startValue <= boxController.boxCount)
        {
            var difference = previousCount - boxController.boxCount;
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(UpdateFieldView(false, difference));
        }
        
        previousCount = boxController.boxCount;
    }

    private IEnumerator UpdateFieldView(bool add, float difference)
    {
        var working = true;
        
        while (working)
        {
            if (!add)
                camera.fieldOfView -= Mathf.Lerp(0, difference * distance, t);
            else
                camera.fieldOfView += Mathf.Lerp(0, difference * distance, t);

            t += bias * Time.deltaTime;
            
            if (t > 1)
            {
                t = 0.0f;
                working = false;
            }
            
            yield return null;
        }
    }
}
