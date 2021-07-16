using UnityEngine;

public class StartingRoad : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    
    private void Awake() => meshRenderer = GetComponent<MeshRenderer>();

    public Vector3 GetStartPosition(MeshRenderer playerMeshRenderer)
    {
        var meshBounds = meshRenderer.bounds;
        var centerPoint = meshBounds.center;
        var minBounds = meshBounds.min;
        var maxBounds = meshBounds.max;
        var offsetY = Mathf.Abs(maxBounds.y - minBounds.y) / 2;
        var additionalOffsetY = GetAdditionalOffsetY(playerMeshRenderer);
        var yCord = centerPoint.y + offsetY + additionalOffsetY;
        return new Vector3(centerPoint.x, yCord, centerPoint.z);
    }

    private float GetAdditionalOffsetY(MeshRenderer playerMeshRenderer)
    {
        var meshBounds = playerMeshRenderer.bounds;
        return Mathf.Abs(meshBounds.max.y - meshBounds.min.y) / 2;
    }
}