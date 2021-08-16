using System;
using System.Collections.Generic;
using DefaultNamespace.ThemeManager;
using Services.StartBoxCountManager;
using UnityEngine;
using Zenject;

public class BoxController : MonoBehaviour
{
    public delegate void BoxChanged(bool finish, int multiplier);
    public event Action AddedBox;
    public event BoxChanged RemovedBox;

    private int startCountBoxes;
    
    //TODO refactor me
    [SerializeField] private GameObject friendlyBox;
    [SerializeField] private GameObject road;
    [SerializeField] private Transform trail;
    public float height;
    private List<FriendlyBox> boxes;
    private Bounds BoxBounds => friendlyBox.GetComponent<MeshRenderer>().bounds;
    private Bounds GroundBounds => road.GetComponent<MeshRenderer>().bounds;
    private float offsetYForGround => Mathf.Abs(GroundBounds.max.y - GroundBounds.center.y);
    
    private StartingRoad startingRoad;
    private Transform currentRoad;

    public int boxCount => transform.childCount - 1;
    private float heightBox => Mathf.Abs(BoxBounds.max.y - BoxBounds.min.y);

    private BoxAudioController boxAudioController;
    private StartBoxCountManager startBoxCountManager;
    private ThemeManager themeManager;
    private GameController gameController;
    
    [Inject]
    private void Construct(StartingRoad startingRoad, BoxAudioController boxAudioController, 
        StartBoxCountManager startBoxCountManager, ThemeManager themeManager, GameController gameController)
    {
        this.startingRoad = startingRoad;
        this.boxAudioController = boxAudioController;
        this.startBoxCountManager = startBoxCountManager;
        this.themeManager = themeManager;
        this.gameController = gameController;
    }

    private void Awake()
    {
        height = heightBox;
        boxes = new List<FriendlyBox>();
        startCountBoxes = startBoxCountManager.GetData();
        
        for (var i = 0; i < startCountBoxes; i++)
        {
            var instance = Instantiate(friendlyBox);
            instance.transform.SetParent(transform);
            instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController);
        }

        for (var i = 0; i < transform.childCount; i++)
            AddBox(transform.GetChild(i).gameObject);

        transform.position = new Vector3(startingRoad.GetStartPosition().x,
            transform.position.y, startingRoad.GetStartPosition().z);
    }

    public void ClearBoxes()
    {
        var gameObjects = new List<GameObject>();
        
        for (var i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (var box in gameObjects)
        {
            Destroy(box);
            //box.transform.SetParent(null);
            //boxes.Remove(box.GetComponent<FriendlyBox>());
            
        }
    }
    
    public void InstantiateNewBox()
    {
        var instance = Instantiate(friendlyBox);
        instance.transform.SetParent(transform);
        instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController);
        AddBox(instance);
    }
    
    public void SpecialAddBox(GameObject box)
    {
        boxes.Add(box.GetComponent<FriendlyBox>());
        box.transform.rotation = Quaternion.Euler(transform.parent.rotation.eulerAngles.x, 
            transform.parent.rotation.eulerAngles.y, transform.parent.rotation.eulerAngles.z);
        box.transform.SetParent(transform);
        CalculateBoxPositions();
    }
    
    public void AddBox(GameObject box)
    {
        boxes.Add(box.GetComponent<FriendlyBox>());
        box.transform.rotation = Quaternion.Euler(transform.parent.rotation.eulerAngles.x, 
            transform.parent.rotation.eulerAngles.y, transform.parent.rotation.eulerAngles.z);
        box.transform.SetParent(transform);
        CalculateBoxPositions();
        AddedBox?.Invoke(); // for camera field view
    }

    public void RemoveBox(GameObject box, bool finish, int multiplier, bool destroy = false)
    {
        box.transform.SetParent(null);
        boxes.Remove(box.GetComponent<FriendlyBox>());
        RemovedBox?.Invoke(finish, multiplier); // for camera field view
        UpdateBoxesTag();
        
        if(destroy)
            Destroy(box);
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

    private void UpdateBoxesTag()
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            if (i == transform.childCount - 1)
            {
                var box = transform.GetChild(transform.childCount - 1);
                box.tag = "DiamondCollector";
            }
            else
            {
                var box = transform.GetChild(i);
                box.tag = "Untagged";
            }
        }
    }
    
    private void CalculateBoxPositions()
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            if (i == transform.childCount - 1)
            {
                var box = transform.GetChild(transform.childCount - 1);
                box.position = new Vector3(transform.position.x,
                    road.transform.position.y + offsetYForGround + heightBox / 2, transform.position.z);

                box.tag = "DiamondCollector";
            }
            else
            {
                var box = transform.GetChild(i);
                box.position = new Vector3(transform.position.x,
                    transform.GetChild(i + 1).position.y + heightBox, transform.position.z);

                box.tag = "Untagged";
            }
        }
    }
}