using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoxController : MonoBehaviour
{
    //TODO refactor me
    [SerializeField] private GameObject road;
    [SerializeField] private Transform trail;
    
    private List<FriendlyBox> boxes;
    private Bounds BoxBounds => transform.GetChild(1).GetComponent<MeshRenderer>().bounds;
    private Bounds GroundBounds => road.GetComponent<MeshRenderer>().bounds;
    private float offsetYForGround => Mathf.Abs(GroundBounds.max.y - GroundBounds.center.y);
    private StartingRoad startingRoad;
    private Transform currentRoad;

    
    public float heightBox => Mathf.Abs(BoxBounds.max.y - BoxBounds.min.y);
    
    [Inject] private void Construct(StartingRoad startingRoad) => this.startingRoad = startingRoad;
    

    private void Awake()
    {
        boxes = new List<FriendlyBox>();

        for (var i = 0; i < transform.childCount; i++)
            AddBox(transform.GetChild(i).gameObject);

        transform.position = new Vector3(startingRoad.GetStartPosition().x,
            transform.position.y, startingRoad.GetStartPosition().z);
    }

    public void AddBox(GameObject box)
    {
        boxes.Add(box.GetComponent<FriendlyBox>());
        box.transform.SetParent(transform);
        CalculateBoxPositions();
    }

    public void RemoveBox(GameObject box)
    {
        box.transform.SetParent(null);
        boxes.Remove(box.GetComponent<FriendlyBox>());
    }

    public void UpdateTrailPosition()
    {
        trail.position = new Vector3(transform.position.x,
            road.transform.position.y + offsetYForGround, transform.position.z);
    }

    public void DisablePhysics()
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().useGravity = false;
            transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void EnablePhysics(bool resetVelocity = false)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
            if (resetVelocity)
                transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void CalculateBoxPositions()
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            if (i == transform.childCount - 1)
            {
                transform.GetChild(transform.childCount - 1).position = new Vector3(transform.position.x,
                    road.transform.position.y + offsetYForGround + heightBox / 2, transform.position.z);
            }
            else
            {
                transform.GetChild(i).position = new Vector3(transform.position.x,
                    transform.GetChild(i + 1).position.y + heightBox, transform.position.z);
                
                UpdateTrailPosition();
            }
        }
    }
}