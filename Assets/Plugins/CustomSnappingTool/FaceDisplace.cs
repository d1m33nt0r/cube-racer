using System.Collections.Generic;
using UnityEngine;

public class FaceDisplace : MonoBehaviour
{
    [SerializeField] private int countSubdivisionZ;
    [SerializeField] private int countSubdivisionX;
    [SerializeField] private Side sideSubdivision;
    
    private MeshRenderer meshRenderer => GetComponent<MeshRenderer>();

    private List<CustomSnapConnector> customSnapConnectors;
    private List<List<Vector3>> subdividedPositions = new List<List<Vector3>>();
    private GameObject connectors;
    
    public void Displace()
    {
        RemoveConnectors();
        
        var bounds = meshRenderer.bounds;

        var min = bounds.min;
        var max = bounds.max;
        
        List<Vector3> faceVertices;

        if (sideSubdivision == Side.Up)
            faceVertices = GetUpFaceVertices(min, max);
        else
            faceVertices = GetDownFaceVertices(min, max);
        
        var zSize = Mathf.Abs(faceVertices[0].z - faceVertices[1].z);
        var xSize = Mathf.Abs(faceVertices[3].x - faceVertices[0].x);
        
        CalculateSubdividedPositions(xSize, zSize, faceVertices);
        CreateCustomSnapConnectors();
    }

    private void CreateCustomSnapConnectors()
    {
        connectors = new GameObject {name = "Connectors"};
        connectors.transform.SetParent(transform);

        foreach (var positions in subdividedPositions)
        {
            foreach (var position in positions)
            {
                var customSnapConnector = CreateSnapConnector().transform;
                customSnapConnector.position = position;
                customSnapConnector.SetParent(connectors.transform);
            }
        }
    }

    private void CalculateSubdividedPositions(float xSize, float zSize, List<Vector3> orderedFaceVertices)
    {
        var xSubdividedSize = xSize / countSubdivisionX;
        var zSubdividedSize = zSize / countSubdivisionZ;

        for (var i = 0; i < countSubdivisionZ; i++)
        {
            subdividedPositions.Add(new List<Vector3>());
            var prevZPoint = new Vector3(orderedFaceVertices[0].x,
                orderedFaceVertices[0].y, orderedFaceVertices[0].z + i * zSubdividedSize);

            for (var j = 0; j < countSubdivisionX; j++)
            {
                subdividedPositions[i].Add(new Vector3(prevZPoint.x + j * xSubdividedSize + xSubdividedSize / 2,
                    prevZPoint.y, prevZPoint.z + zSubdividedSize / 2));
            }
        }
    }

    public void RemoveConnectors()
    {
        if (connectors != null)
        {
            subdividedPositions.Clear();
            DestroyImmediate(connectors);
        }
    }
    
    private List<Vector3> GetUpFaceVertices(Vector3 min, Vector3 max)
    {
        var orderedFaceVertices = new List<Vector3>();

        var firstPoint = new Vector3(min.x, max.y, min.z);
        var secondPoint = new Vector3(min.x, max.y, max.z);
        var thirdPoint = max;
        var fourthPoint = new Vector3(max.x, max.y, min.z);

        orderedFaceVertices.Add(firstPoint);
        orderedFaceVertices.Add(secondPoint);
        orderedFaceVertices.Add(thirdPoint);
        orderedFaceVertices.Add(fourthPoint);

        return orderedFaceVertices;
    }
    
    private List<Vector3> GetDownFaceVertices(Vector3 min, Vector3 max)
    {
        var orderedFaceVertices = new List<Vector3>();

        var firstPoint = min;
        var secondPoint = new Vector3(min.x, min.y, max.z);
        var thirdPoint = new Vector3(max.x, min.y, max.z);
        var fourthPoint = new Vector3(max.x, min.y, min.z);

        orderedFaceVertices.Add(firstPoint);
        orderedFaceVertices.Add(secondPoint);
        orderedFaceVertices.Add(thirdPoint);
        orderedFaceVertices.Add(fourthPoint);

        return orderedFaceVertices;
    }

    private List<Vector3> sGetFaceVertices(List<Vector3> calculatedVertices, Vector3 max)
    {
        var faceVertices = new List<Vector3>();

        foreach (var vertex in calculatedVertices)
        {
            if (vertex.y != max.y) continue;

            if (faceVertices.Contains(vertex)) continue;

            faceVertices.Add(vertex);
        }

        return faceVertices;
    }

    private List<Vector3> ConvertVerticesToGlobalPosition(Mesh mesh)
    {
        var calculatedVertices = new List<Vector3>();
        var vertices = mesh.vertices;

        foreach (var vertex in vertices)
        {
            if (calculatedVertices.Contains(vertex)) continue;

            var calculatedVertexCoordinates = transform.TransformPoint(vertex);
            calculatedVertices.Add(calculatedVertexCoordinates);
        }

        return calculatedVertices;
    }
    
    private CustomSnapConnector CreateSnapConnector()
    {
        var obj = new GameObject {name = "Connector"};
        return obj.AddComponent<CustomSnapConnector>();
    }
    
    private enum Side
    {
        Up,
        Down
    }
}