using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] private float indentBetweenBoxes;
    [SerializeField] private float indentBetweenGround;
    [SerializeField] private Transform player;
    [SerializeField] private Transform trail;
    [SerializeField] private PhysicsManipulator physicsManipulator;
    
    public int boxCount => transform.childCount;
    public float heightBox => Mathf.Abs(BoxBounds.max.y - BoxBounds.min.y);
    
    private List<FriendlyBox> boxes;
    private Bounds BoxBounds => transform.GetChild(0).GetComponent<MeshRenderer>().bounds;
    private Bounds PlayerBounds => player.GetComponent<MeshRenderer>().bounds;
    private float offsetYWithPlayer => Mathf.Abs(PlayerBounds.max.y - PlayerBounds.min.y) / 2;
    
    private void Start()
    {
        boxes = new List<FriendlyBox>();
        CalculateBoxPositions();

        for (var i = 0; i < transform.childCount; i++)
        {
            AddBox(transform.GetChild(i).gameObject);
        }
    }
    
    public void AddBox(GameObject box)
    {
        boxes.Add(box.GetComponent<FriendlyBox>());
        box.transform.parent = transform;
        player.GetComponent<PlayerSetup>().UpdatePosition();
        physicsManipulator.AddRigidBody(box.GetComponent<Rigidbody>());
    }

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
}