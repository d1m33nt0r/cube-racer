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
    public event Action AddedBoxes;
    public event Action SpecialAddedBox;
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
    [SerializeField] private GameObject playerRenderer;
    public int boxCount => transform.childCount - 2;
    private float heightBox => 0.2105f; 

    private BoxAudioController boxAudioController;
    private StartBoxCountManager startBoxCountManager;
    private ThemeManager themeManager;
    private GameController gameController;

    private MagnitPlayer magnitPlayerEffect;

    public int prevBoxCount;
    
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
            var instance = Instantiate(friendlyBox, Vector3.zero, Quaternion.AngleAxis(90, Vector3.up));
            instance.transform.SetParent(transform);
            instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController);
            AddBox(instance);
        }

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
        prevBoxCount = boxCount;
        
        for (var i = 0; i < count; i++)
        {
            var instance = Instantiate(friendlyBox);
            instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController);
            
            AddBox(instance);
        }

        SpecialAddedBox?.Invoke();
    }

    public void BoxGroupAdded(int countBoxes)
    {
        //DisablePhysics();
        AddedBoxes?.Invoke(); // for camera field view
    }

    public void AddBox(GameObject box)
    {
        box.transform.rotation = Quaternion.Euler(transform.parent.rotation.eulerAngles.x, 
            transform.parent.rotation.eulerAngles.y, transform.parent.rotation.eulerAngles.z);
        box.transform.SetParent(transform);
        
        Vector3 tempPos;
        if (boxes.Count == 0)
        {
            tempPos = new Vector3(road.transform.position.x, 
                road.transform.position.y + offsetYForGround + heightBox / 2, road.transform.position.z);
        }
        else
        {
            tempPos = boxes[boxes.Count - 1].transform.position;
        }
        
        box.transform.position = new Vector3(tempPos.x, tempPos.y + heightBox, tempPos.z);
        boxes.Add(box.GetComponent<FriendlyBox>());
        
        playerRenderer.transform.position = new Vector3(tempPos.x, tempPos.y + heightBox * 2, tempPos.z);
    }

    public void RemoveBox(GameObject box, bool finish, int multiplier, bool destroy = false)
    {
        box.transform.SetParent(null);
        boxes.Remove(box.GetComponent<FriendlyBox>());
        RemovedBox?.Invoke(finish, multiplier); // for camera field view
   
        if(destroy)
            Destroy(box);
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