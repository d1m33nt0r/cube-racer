using UnityEngine;
using Zenject;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private BoxController boxController;
    
    private StartingRoad startingRoad;
    private MeshRenderer playerMeshRenderer;

    [Inject] private void Construct(StartingRoad startingRoad) => this.startingRoad = startingRoad;

    private void Start()
    {
        playerMeshRenderer = GetComponent<MeshRenderer>();
        transform.position = startingRoad.GetStartPosition(playerMeshRenderer);
        var position = transform.position;
        transform.position = new Vector3(position.x, position.y + GetBoxesOffset(), position.z);
    }

    public void UpdatePosition()
    {
        var position = transform.position;
        
        transform.position = new Vector3(position.x,
            position.y + boxController.heightBox, position.z);
        
        boxController.CalculateBoxPositions();
    }
    
    private float GetBoxesOffset() => boxController.heightBox * boxController.boxCount;
}