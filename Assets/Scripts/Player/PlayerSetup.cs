using UnityEngine;
using Zenject;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private BoxController boxController;
    
    private StartingRoad startingRoad;
    private MeshRenderer playerMeshRenderer;

    [Inject] private void Construct(StartingRoad startingRoad) => this.startingRoad = startingRoad;

    public Vector3 UpdatePosition()
    {
        var upperPosition = boxController.transform.GetChild(0).position;
        var playerOffsetY = 0.2f;
        return new Vector3(
            startingRoad.GetStartPosition().x, 
            upperPosition.y + playerOffsetY, 
            startingRoad.GetStartPosition().z);
    }
}