using UnityEngine;

public class PovorotNeTudaSpline : MonoBehaviour
{
    [SerializeField] private Transform P0;
    [SerializeField] private Transform P1;
    [SerializeField] private Transform P2;
    [SerializeField] private Transform P3;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private bool moving;
    [SerializeField] private float speed = 3f;

    [Range(0, 1)] [SerializeField] private float t;

    private float minLimiter = -0.4f, maxLimiter = 0.4f;
    private float previousZ, previousX;

    private float t3;
    private float prevDeltaLeft, prevDeltaRight;

    private void Update()
    {
        if (moving)
        {
            t = Mathf.Lerp(0,1, t3);
            t3 += speed * Time.deltaTime;

            targetTransform.position = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, t);
            targetTransform.rotation = Quaternion
                .LookRotation(Bezier.GetFirstDerivative(P0.position, P1.position, P2.position, P3.position, t));
        }
    }

    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }

    private void OnDrawGizmos() 
    {
        if (P0 == null)
            return;

        var sigmentsNumber = 20;
        var preveousePoint = P0.position;

        for (var i = 0; i < sigmentsNumber + 1; i++) 
        {
            var paremeter = (float)i / sigmentsNumber;
            var point = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, paremeter);
            Debug.DrawLine(preveousePoint, point, Color.black);
            preveousePoint = point;
        }
    }
}