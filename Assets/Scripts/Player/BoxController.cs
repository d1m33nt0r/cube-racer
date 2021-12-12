using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.ThemeManager;
using Services.StartBoxCountManager;
using UnityEngine;
using Zenject;

public class BoxController : MonoBehaviour
{
    public delegate void BoxChanged(bool finish, int multiplier);
    public event Action<int> AddedBoxes;
    public event Action SpecialAddedBox;
    public event BoxChanged RemovedBox;

    private int startCountBoxes;
    
    //TODO refactor me
    [SerializeField] private GameObject friendlyBox;
    [SerializeField] private GameObject road;
    [SerializeField] private Transform trail;
    [SerializeField] private Color emissionColorEffect;
    [SerializeField] private Color emissionStartColorEffect;
    [SerializeField] private float specialAddBoxAnimationSpeed;
    public Vector3 LocalPosition => transform.localPosition;
    
    private List<FriendlyBox> boxes;
    public List<FriendlyBox> Boxes => boxes;
    private Bounds BoxBounds => friendlyBox.GetComponent<MeshRenderer>().bounds;
    private Bounds GroundBounds => road.GetComponent<MeshRenderer>().bounds;
    private float offsetYForGround => Mathf.Abs(GroundBounds.max.y - GroundBounds.center.y);
    
    private StartingRoad startingRoad;
    private Transform currentRoad;
    [SerializeField] private GameObject playerRenderer;
    public int boxCount => transform.childCount - 2;
    public float heightBox => 0.2105f; 

    private BoxAudioController boxAudioController;
    private StartBoxCountManager startBoxCountManager;
    private ThemeManager themeManager;
    private GameController gameController;
    private Vibrator vibrator;
    private MagnitPlayer magnitPlayerEffect;

    public int prevBoxCount;
    
    [Inject]
    private void Construct(StartingRoad startingRoad, BoxAudioController boxAudioController, 
        StartBoxCountManager startBoxCountManager, ThemeManager themeManager, GameController gameController, Vibrator _vibrator)
    {
        this.startingRoad = startingRoad;
        this.boxAudioController = boxAudioController;
        this.startBoxCountManager = startBoxCountManager;
        this.themeManager = themeManager;
        this.gameController = gameController;
        vibrator = _vibrator;
    }

    private void Awake()
    {
        magnitPlayerEffect = transform.parent.GetComponentInChildren<MagnitPlayer>();
        boxes = new List<FriendlyBox>();
        startCountBoxes = startBoxCountManager.GetData();
        
        for (var i = 0; i < startCountBoxes; i++)
        {
            var instance = Instantiate(friendlyBox, Vector3.zero, Quaternion.AngleAxis(90, Vector3.up));
            instance.transform.SetParent(transform);
            instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController, vibrator);
            AddBox(instance);
        }

        CalculateBoxPositions();
        
        transform.position = new Vector3(startingRoad.GetStartPosition().x,
            transform.position.y, startingRoad.GetStartPosition().z);
    }

    public FriendlyBox GetLastElement()
    {
        FriendlyBox _box = null;
        var i = 0;
        foreach (var box in boxes)
        {
            if (i == 0)
            {
                _box = box;
                i++;
                continue;
            }

            if (box.transform.position.y > _box.transform.position.y)
            {
                _box = box;
            }
        }

        return _box;
    }
    
    public bool IsLastElement(FriendlyBox _friendlyBox)
    {
        if (boxes.Last() == _friendlyBox) return true;
        
        return false;
    }
    
    public void ClearBoxes()
    {
        for (var i = 2; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    
    public void InstantiateNewBox()
    {
        var instance = Instantiate(friendlyBox);
        instance.transform.SetParent(transform);
        instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController, vibrator);
        AddBox(instance);
    }

    public void DisablePhysics()
    {
        foreach (var box in boxes)
        {
            box.GetComponent<Rigidbody>().useGravity = false;
        }
    }
    
    public void SpecialAddBox(int count)
    {
        prevBoxCount = boxCount;
        
        for (var i = 0; i < count; i++)
        {
            var instance = Instantiate(friendlyBox);
            instance.GetComponent<FriendlyBox>().Construct(this, boxAudioController, themeManager, gameController, vibrator);
            
            AddBox(instance);
        }
        AnimateEmission();
        SpecialAddedBox?.Invoke();
    }

    private void AnimateEmission()
    {
        var originalMaterial = boxes[0]?.GetComponent<Renderer>().sharedMaterial;
        if (originalMaterial == null) return;
        var copyMaterial = new Material(originalMaterial);
        var targetRenderers = new Renderer[boxes.Count];
        
        for (var i = 0; i < boxes.Count; i++)
        {
            boxes[i].GetComponent<Renderer>().sharedMaterial = copyMaterial;
            targetRenderers[i] = boxes[i].GetComponent<Renderer>();
        }
        
        var hasEmissionProperty = copyMaterial.HasProperty("_TCP2_AMBIENT_BACK");
        if (!hasEmissionProperty) return;

        StartCoroutine(Animate(copyMaterial, originalMaterial, targetRenderers, 
            (_originalMaterial, _renderers) =>
            {
                foreach (var renderer in _renderers)
                {
                    renderer.sharedMaterial = _originalMaterial;
                }
            }));
    }

    public delegate void ChangeMaterial(Material _material, Renderer[] _targetRenderers);
    
    private IEnumerator Animate(Material _copyMaterial, Material _originalMaterial, Renderer[] _targetRenderers, ChangeMaterial _changeDefaultMaterialAction)
    {
        var emissionColor = emissionStartColorEffect;
        var targetColor = emissionColorEffect;

        float t = 0.0f;

        string param;
        
        switch (TurnState.state)
        {
            case TurnState.State.Forward:
                param = "_TCP2_AMBIENT_BACK";
                break;
            case TurnState.State.Left:
                param = "_TCP2_AMBIENT_RIGHT";
                break;
            case TurnState.State.Right:
                param = "_TCP2_AMBIENT_LEFT";
                break;
            default:
                param = "";
                break;
        }
        
        while (t < 1f)
        {
            _copyMaterial.SetColor(param, new Color(
                Mathf.Lerp(emissionColor.r, targetColor.r, t), 
                Mathf.Lerp(emissionColor.g, targetColor.g, t), 
                Mathf.Lerp(emissionColor.b, targetColor.b, t),
                Mathf.Lerp(emissionColor.a, targetColor.a, t)
                ));
            
            t += specialAddBoxAnimationSpeed * Time.deltaTime;

            yield return null;
        }
        
        _changeDefaultMaterialAction?.Invoke(_originalMaterial, _targetRenderers);
    }
    
    public void BoxGroupAdded(int countBoxes)
    {
        //DisablePhysics();
        AddedBoxes?.Invoke(countBoxes); // for camera field view
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
            tempPos = GetLastElement().transform.position;
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

        box.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        
        if(destroy)
            Destroy(box);
    }

    public Vector3 GetBoxPositionXYZ()
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