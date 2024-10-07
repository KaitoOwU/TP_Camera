using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPositionFollow : AView
{

    [Range(-180f, 180f)] public float roll;
    [Range(0.0f, 180f)] public float fov;
    public Transform target;

    public GameObject centralPoint;
    [Range(0.0f, 360f)] public float yawOffsetMax;
    [Range(-90f, 90f)] public float pitchOffsetMax;
    
    
    public override CameraConfiguration GetConfiguration()
    {
        Vector3 centralDir = (centralPoint.transform.position - transform.position).normalized;
        float centralYaw = Mathf.Atan2(centralDir.x, centralDir.z) * Mathf.Rad2Deg;
        float centralPitch = -Mathf.Asin(centralDir.y) * Mathf.Rad2Deg;
        
        return new CameraConfiguration()
        {
            yaw = centralYaw,
            roll = this.roll,
            pitch = centralPitch,
            fov = this.fov,
            distance = 0f,
            pivot = transform.position
        };
    }
    
    
}
