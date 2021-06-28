using System;
using DefaultNamespace;
using UnityEngine;

public class MeshScanerPoint : MonoBehaviour
{
    [SerializeField] private Backlight _backlight;
    [SerializeField] private Transform player;

    private Vector3 normal;
    private float angle;
    private float hitDistance;
    private bool isRightTurn;
    private float prevAngle;

    private Collider _collider;
    
    private void Start()
    {
        var playerPosition = player.position;
        var originPosition = transform.position;
        var direction = new Vector3(playerPosition.x,
            playerPosition.y - 0.6f, playerPosition.z) - originPosition;
        
        prevAngle = 0;

        RaycastHit hit;

        if (Physics.Raycast(originPosition, direction, out hit, Mathf.Infinity))
            hitDistance = hit.distance;
    }

    private void FixedUpdate()
    {
        Raycast();
    }

    private void Raycast()
    {
        var playerPosition = player.position;
        var originPosition = transform.position;
        
        var direction = new Vector3(playerPosition.x,
            playerPosition.y - 0.6f, playerPosition.z) - originPosition;

        RaycastHit hit;
        
        if (Physics.Raycast(originPosition, direction, out hit, Mathf.Infinity))
        {
            if (_collider != hit.collider)
            {
                if (_collider != null)
                    _collider.enabled = false;
                
                _collider = hit.collider;
            }
            
            Debug.DrawRay(originPosition, direction, Color.red);

            if (hitDistance != hit.distance)
            {
                if (hitDistance > hit.distance)
                {
                    isRightTurn = true;
                }
                else
                {
                    isRightTurn = false;
                }
                
                hitDistance = hit.distance; 
            }
            
            var ray = new RayParams(originPosition, direction);
            var mesh = hit.transform.GetComponent<MeshFilter>().mesh;
            var triangle = new Vector3[3];
            
            for (var i = 0; i < mesh.triangles.Length; i += 3)
            {
                triangle[0] = hit.transform.TransformPoint(mesh.vertices[mesh.triangles[i]]);
                triangle[1] = hit.transform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]);
                triangle[2] = hit.transform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]);

                if (IntersectTriangle(ray, triangle[0], triangle[1], triangle[2]))
                {
                    normal = Vector3.Cross(triangle[1] - triangle[0], triangle[2] - triangle[0]);
                    angle = Vector3.Angle(normal, originPosition - new Vector3(playerPosition.x,
                        playerPosition.y - 0.6f, playerPosition.z));

                    if (prevAngle < angle && isRightTurn)
                    {
                        //Time.timeScale = 0.05f;
                        player.GetComponent<PlayerMover>().Rotate(angle);
                        Debug.Log("Angle " + angle);
                        Debug.Log("Prev Angle " + prevAngle);
                        Debug.Log("Normal " + normal);
                        Debug.Log("Hit distance " + hit.distance);
                    }

                    Debug.DrawRay(triangle[0], normal, Color.blue);
                    
                    if (_backlight != Backlight.Disabled && _backlight != Backlight.AllTriangles)
                        DrawTriangle(triangle);
                }

                if (_backlight != Backlight.Disabled && _backlight != Backlight.OnlyHitTriangle)
                    DrawMeshTriangles(mesh, hit);
            }
        }
    }

    private void DrawMeshTriangles(Mesh mesh, RaycastHit hit)
    {
        var triangle = new Vector3[3];
        
        for (var i = 0; i < mesh.triangles.Length; i += 3)
        {
            triangle[0] = hit.transform.TransformPoint(mesh.vertices[mesh.triangles[i]]);
            triangle[1] = hit.transform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]);
            triangle[2] = hit.transform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]);

            DrawTriangle(triangle);
        }
    }

    private void DrawTriangle(Vector3[] triangle)
    {
        Debug.DrawRay(triangle[0], triangle[1] - triangle[0], Color.magenta);
        Debug.DrawRay(triangle[1], triangle[2] - triangle[1], Color.magenta);
        Debug.DrawRay(triangle[2], triangle[0] - triangle[2], Color.magenta);
    }

    public struct RayParams
    {
        private Vector3 _direction;
        private Vector3 _origin;

        public RayParams(Vector3 origin, Vector3 direction)
        {
            _direction = direction;
            _origin = origin;
        }

        public Vector3 direction => _direction;
        public Vector3 origin => _origin;
    }

    public bool IntersectTriangle(RayParams ray, Vector3 vert0, Vector3 vert1, Vector3 vert2)
    {
        var edge1 = vert1 - vert0;
        var edge2 = vert2 - vert0;
        
        var pvec = Vector3.Cross(ray.direction, edge2);
        
        var det = Vector3.Dot(edge1, pvec);
        
        if (det < Mathf.Epsilon)
            return false;

        var inv_det = 1.0f / det;
        
        var tvec = ray.origin - vert0;
        
        var u = Vector3.Dot(tvec, pvec) * inv_det;

        if (u < 0.0 || u > 1.0f)
            return false;
        
        var qvec = Vector3.Cross(tvec, edge1);
        
        var v = Vector3.Dot(ray.direction, qvec) * inv_det;

        if (v < 0.0 || u + v > 1.0f)
            return false;

        return true;
    }
}

public enum Backlight
{
    AllTriangles,
    OnlyHitTriangle,
    Disabled
}