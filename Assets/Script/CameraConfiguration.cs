using System;
using UnityEngine;

[Serializable]
public struct CameraConfiguration
{
    [Range(0.0f, 360f)] public float yaw;
    [Range(-90f, 90f)] public float pitch;
    [Range(-180f, 180f)] public float roll;
    public Vector3 pivot;
    [Min(0f)] public float distance;
    [Range(0.0f, 180f)] public float fov;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, roll);
    }

    public Vector3 GetPosition()
    {
        return pivot + GetRotation()*(Vector3.back*distance);
    }
    
    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pivot, 0.25f);
        Vector3 position = GetPosition();
        Gizmos.DrawLine(pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
