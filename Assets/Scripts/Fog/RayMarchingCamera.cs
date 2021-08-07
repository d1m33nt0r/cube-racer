using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RayMarchingCamera : MonoBehaviour
{
    private Matrix4x4 frustumCorners = Matrix4x4.identity;
    public Material material;
    public Camera myCamera;

    public Transform cameraTransform;

    // Use this for initialization
    private void Start()
    {
        myCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //field of view
        float fov = myCamera.fieldOfView;
        //Distance near cutting surface
        float near = myCamera.nearClipPlane;
        //Horizontal to vertical ratio
        float aspect = myCamera.aspect;
        //Height close to half of cutting surface
        float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        //Vectors up and right
        Vector3 toRight = myCamera.transform.right * halfHeight * aspect;
        Vector3 toTop = myCamera.transform.up * halfHeight;

        //The vectors from the camera to the four corners near the cutting surface are obtained respectively
        //depth/dist=near/|topLeft|
        //dist=depth*(|TL|/near)
        //scale=|TL|/near
        Vector3 topLeft = cameraTransform.forward * near + toTop - toRight;
        float scale = topLeft.magnitude / near;

        topLeft.Normalize();
        topLeft *= scale;

        Vector3 topRight = cameraTransform.forward * near + toTop + toRight;
        topRight.Normalize();
        topRight *= scale;

        Vector3 bottomLeft = cameraTransform.forward * near - toTop - toRight;
        bottomLeft.Normalize();
        bottomLeft *= scale;

        Vector3 bottomRight = cameraTransform.forward * near - toTop + toRight;
        bottomRight.Normalize();
        bottomRight *= scale;

        //Assign a value to a matrix
        frustumCorners.SetRow(0, bottomLeft);
        frustumCorners.SetRow(1, bottomRight);
        frustumCorners.SetRow(2, topRight);
        frustumCorners.SetRow(3, topLeft);
        //Pass the vector to the fixed-point shader, and pass the screen image to a shader
        material.SetMatrix("_FrustumCornorsRay", frustumCorners);
        material.SetTexture("_MainTex", source);
        Graphics.Blit(source, destination, material, 0);
    }
}