using DefaultNamespace;
using UnityEngine;

public class StartingRoad : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    private void Start()
    {
        StartPoint.SetStartPoint(GetStartPosition());
    }

    public Vector3 GetStartPosition()
    {
        var meshBounds = meshRenderer.bounds;
        var centerPoint = meshBounds.center;
        var minBounds = meshBounds.min;
        var maxBounds = meshBounds.max;
        var offsetY = Mathf.Abs(maxBounds.y - minBounds.y) / 2;
        return new Vector3(centerPoint.x, centerPoint.y + offsetY, centerPoint.z);
    }
}