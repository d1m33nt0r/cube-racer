using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.ThemeManager;
using Services.StartBoxCountManager;
using UnityEngine;
using Zenject;

public class BoxController : MonoBehaviour
{
    public delegate void BoxChanged(bool finish, int multiplier);
    public event Action<int> AddedBoxes;
    public event BoxChanged RemovedBox;

    private int startCountBoxes;
    
    //TODO refactor me
    [SerializeField] private GameObject friendlyBox;
    [SerializeField] private GameObject road;
    [SerializeField] private Transform trail;
    public float height;
    private List<FriendlyBox> boxes;
    public List<FriendlyBox> Boxes => boxes;
    private Bounds BoxBounds => friendlyBox.GetComponent<MeshRenderer>().bounds;
    private Bounds GroundBounds => road.GetComponent<MeshRenderer>().bounds;
    private float offsetYForGround => Mathf.Abs(GroundBounds.max.y - GroundBounds.center.y);
    
    private StartingRoad startingRoad;
    private Transform currentRoad;

    public int boxCount => transform.childCount - 2;
    private float heightBox => 0.21f; //Mathf.Abs(BoxBounds.max.y - BoxBounds.min.y);

    private BoxAudioController boxAudioController;
    private StartBoxCountManager startBoxCountManager;
    private ThemeManager themeManager;
    private GameController gameController;

    private MagnitPlayer magnitPlayerEffect;

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
        magnitPlayerEffect = transform.parent.GetComponentInChildren<MagnitPlayer>();
        height = heightBox;
        boxes = new List<FriendlyBox>();
        startCountBoxes = startBoxCountManager.GetData();
        
        for (var i = 0; i < startCountBoxes; i++)
        {
            var instance = Instantiate(friendlyBox);
            instance.transform.SetParent(transform);
            instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController);
        }

        for (var i = 1; i < transform.childCount; i++)
            AddBox(transform.GetChild(i).gameObject);

        CalculateBoxPositions();
        
        transform.position = new Vector3(startingRoad.GetStartPosition().x,
            transform.position.y, startingRoad.GetStartPosition().z);
    }

    public bool IsLastElement(FriendlyBox _friendlyBox)
    {
        if (boxes.Last() == _friendlyBox) return true;
        
        return false;
    }
    
    public void ClearBoxes()
    {
        for (var i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    
    public void InstantiateNewBox()
    {
        var instance = Instantiate(friendlyBox);
        instance.transform.SetParent(transform);
        instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController);
        AddBox(instance);
    }
    
    public void SpecialAddBox(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var instance = Instantiate(friendlyBox);
            instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController);
            boxes.Add(instance.GetComponent<FriendlyBox>());
            instance.transform.rotation = Quaternion.Euler(transform.parent.rotation.eulerAngles.x, 
                transform.parent.rotation.eulerAngles.y, transform.parent.rotation.eulerAngles.z);
            instance.transform.SetParent(transform);
        }
        
        CalculateBoxPositions();
    }

    public void BoxGroupAdded(int countBoxes)
    {
        AddedBoxes?.Invoke(countBoxes); // for camera field view
    }

    public void AddBox(GameObject box)
    {
        boxes.Add(box.GetComponent<FriendlyBox>());
        box.transform.rotation = Quaternion.Euler(transform.parent.rotation.eulerAngles.x, 
            transform.parent.rotation.eulerAngles.y, transform.parent.rotation.eulerAngles.z);
        box.transform.SetParent(transform); 
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
        for (var i = transform.childCount - 1; i >= 1; i--)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().useGravity = false;
            transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void EnablePhysics(bool resetVelocity = false)
    {
        for (var i = transform.childCount - 1; i >= 1; i--)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
            if (resetVelocity)
                transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void UpdateBoxesTag()
    {
        for (var i = transform.childCount - 1; i >= 1; i--)
        {
            if (i == transform.childCount - 1)
            {
                var box = transform.GetChild(transform.childCount - 1);
                box.tag = "Untagged";
            }
            else
            {
                var box = transform.GetChild(i);
                box.tag = "Untagged";
            }
        }
    }

    public Vector3 GetBoxPositionXZ()
    {
        return boxes[boxes.Count - 1].transform.position;
    }
    
    public void CalculateBoxPositions()
    {
        for (var i = transform.childCount - 1; i >= 1; i--)
        {
            if (i == transform.childCount - 1)
            {
                var box = transform.GetChild(transform.childCount - 1);
                box.position = new Vector3(transform.position.x,
                    road.transform.position.y + offsetYForGround + heightBox / 2, transform.position.z);

                box.tag = "Untagged";
            }
            else
            {
                var prevBoxIndex = i + 1;
                var box = transform.GetChild(i);
                box.position = new Vector3(transform.position.x,
                    transform.GetChild(prevBoxIndex).position.y + heightBox, transform.position.z);

                box.tag = "Untagged";
            }
        }
    }
}