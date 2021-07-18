using UnityEngine;

public class DiamondAnimation : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distance;
    [SerializeField] private float angle;

    private float currentAngle;
    private Vector3 upPoint;
    private Vector3 downPoint;

    private float t = 0.0f;
    
    private void Start()
    {
        var position = transform.position;
        upPoint = new Vector3(position.x, position.y + distance / 2, position.z);
        downPoint = new Vector3(position.x, position.y - distance / 2, position.z);
    }

    void Update()
    {
        transform.rotation = Quaternion.AngleAxis(currentAngle += angle, Vector3.up);
        
        transform.position = new Vector3(transform.position.x, 
            Mathf.Lerp(downPoint.y, upPoint.y, t), transform.position.z);
        
        t += moveSpeed * Time.deltaTime;
        
        if (t > 1.0f)
        {
            float temp = upPoint.y;
            upPoint.y = downPoint.y;
            downPoint.y = temp;
            t = 0.0f;
        }
        
    }
}
