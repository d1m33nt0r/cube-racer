using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoxController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform trail;

    //TODO inject
    [SerializeField] private GameObject road;

    public int boxCount => transform.childCount;
    public float heightBox => Mathf.Abs(BoxBounds.max.y - BoxBounds.min.y);

    private List<FriendlyBox> boxes;
    private Bounds BoxBounds => transform.GetChild(1).GetComponent<MeshRenderer>().bounds;
    private Bounds PlayerBounds => player.GetComponent<MeshRenderer>().bounds;
    private float offsetYForPlayer => Mathf.Abs(PlayerBounds.max.y - PlayerBounds.min.y) / 2;
    private Bounds GroundBounds => road.GetComponent<MeshRenderer>().bounds;
    private float offsetYForGround => Mathf.Abs(GroundBounds.max.y - GroundBounds.center.y);

    private StartingRoad startingRoad;

    [Inject]
    private void Construct(StartingRoad startingRoad)
    {
        this.startingRoad = startingRoad;
    }

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


/*
public void CalculateBoxPositions()
    {
        var playerPosition = player.transform.position;

        for (var i = 0; i < transform.childCount; i++)
        {
            if (i == 0)
            {
                transform.GetChild(i).position = new Vector3(playerPosition.x, 
                    playerPosition.y - offsetYWithPlayer * 2, playerPosition.z); 
            }
            else
            {
                transform.GetChild(i).position = new Vector3(playerPosition.x, 
                    playerPosition.y - i * heightBox - offsetYWithPlayer * 2, playerPosition.z); 
            }

            if (i + 1 == transform.childCount)
            {
                trail.position = new Vector3(playerPosition.x,
                    playerPosition.y - i * heightBox - offsetYWithPlayer * 2 - heightBox / 2, playerPosition.z);
            }
        }
    }
*/


/*
public class BoxController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform trail;
    
    //TODO inject
    [SerializeField] private GameObject road;
    
    public int boxCount => transform.childCount;
    public float heightBox => Mathf.Abs(BoxBounds.max.y - BoxBounds.min.y);
    
    private List<FriendlyBox> boxes;
    private Bounds BoxBounds => transform.GetChild(0).GetComponent<MeshRenderer>().bounds;

    private Bounds GroundBounds => road.GetComponent<MeshRenderer>().bounds;
    private float offsetYForGround => Mathf.Abs(GroundBounds.max.y - GroundBounds.min.y) / 2;
    
    private void Start()
    {
        boxes = new List<FriendlyBox>();
        

        for (var i = 0; i < transform.childCount; i++)
            AddBox(transform.GetChild(i).gameObject);
        
        CalculateBoxPositions();
    }

    public void AddBox(GameObject box)
    {
        boxes.Add(box.GetComponent<FriendlyBox>());
        box.transform.SetParent(transform);
        player.GetComponent<PlayerSetup>().UpdatePosition();
    }

    public void RemoveBox(GameObject box)
    {
        boxes.Remove(box.GetComponent<FriendlyBox>());
    }

    public void DisablePhysics()
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().mass = 0;
            transform.GetChild(i).GetComponent<Rigidbody>().useGravity = false;
            transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    
    public void EnablePhysics(bool resetVelocity = false)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().mass = 0.5f;
            transform.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
            if (resetVelocity)
                transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    
    public void CalculateBoxPositions()
    {
        var roadPosition = road.transform.position;

        var j = 0;
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            if (i != transform.childCount - 1)
            {
                transform.GetChild(i).position = new Vector3(transform.position.x, 
                    roadPosition.y + j * heightBox + heightBox / 2 + offsetYForGround, transform.position.z); 
            }

            if (i == 0)
            {
                transform.GetChild(transform.childCount - 1).position = new Vector3(transform.position.x, 
                    roadPosition.y + offsetYForGround + heightBox / 2, transform.position.z);
                
                //trail.position = new Vector3(transform.position.x,
                //    roadPosition.y - i * heightBox - offsetYWithPlayer * 2 - heightBox / 2, transform.position.z);
            }

            j++;
        }
    }
    
    
}
*/