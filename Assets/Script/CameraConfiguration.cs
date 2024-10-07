using System;
using Unity.VisualScripting.FullSerializer;
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
        return pivot + GetRotation() * (Vector3.back * distance);
    }

    public static CameraConfiguration LerpConfig(CameraConfiguration a, CameraConfiguration b, float T)
    {
        return new CameraConfiguration()
        {
            yaw = Vector2.SignedAngle(Vector2.right, Vector2.Lerp(new Vector2(Mathf.Cos(a.yaw * Mathf.Deg2Rad), Mathf.Sin(a.yaw * Mathf.Deg2Rad)), 
                                                                  new Vector2(Mathf.Cos(b.yaw * Mathf.Deg2Rad), Mathf.Sin(b.yaw * Mathf.Deg2Rad)), T)),
            roll = Mathf.Lerp(a.roll, b.roll, T),
            pitch = Mathf.Lerp(a.pitch, b.pitch, T),
            fov = Mathf.Lerp(a.fov, b.fov, T),
            distance = Mathf.Lerp(a.distance, b.distance, T),
            pivot = Vector3.Lerp(a.pivot, b.pivot, T),
        };
    }


    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pivot, 0.1f);
        Vector3 position = GetPosition();
        Gizmos.DrawLine(pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }

    public static float operator-(CameraConfiguration a, CameraConfiguration b)
    {
        Vector3 posDif = b.GetPosition() - a.GetPosition();
        Quaternion rotaDif = b.GetRotation() * Quaternion.Inverse(a.GetRotation());
        return posDif.magnitude +
               rotaDif.x + rotaDif.y + rotaDif.z + rotaDif.w;
    }
}
