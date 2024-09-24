using System;
using UnityEngine;

[Serializable]
public struct CameraConfiguration
{
    [Range(0.0f, 360f)] public float yaw;
    [Range(-90f, 90f)] public float pitch;
    [Range(-180f, 180f)] public float roll;
    public Vector3 pivot;
    [Range(0.0f, float.MaxValue)] public float distance;
    [Range(0.0f, 180f)] public float fov;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, roll);
    }

    public Vector3 GetPosition()
    {
        return pivot + GetRotation()*(Vector3.back*distance);
    }
}
