using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPositionFollow : AView
{

    [Range(-180f, 180f)] public float roll;
    [Range(0.0f, 180f)] public float fov;
    public Transform target;

    public GameObject centralPoint;
    [Range(0.0f, 180f)] public float yawOffsetMax;
    [Range(0, 90f)] public float pitchOffsetMax;

    public override CameraConfiguration GetConfiguration()
    {
        Vector3 centralDir = centralPoint.transform.position - transform.position;
        Vector3 targetDir = target.transform.position - transform.position;
        
        float centralYaw = Mathf.Atan2(centralDir.normalized.x, centralDir.normalized.z) * Mathf.Rad2Deg;
        float targetYaw = Mathf.Atan2(targetDir.normalized.x, targetDir.normalized.z) * Mathf.Rad2Deg;
        
        float centralPitch = -Mathf.Asin(centralDir.normalized.y) * Mathf.Rad2Deg;
        float targetPitch = -Mathf.Asin(targetDir.normalized.y) * Mathf.Rad2Deg;

        float yawDiff = targetYaw - centralYaw;
        while (yawDiff is > 180f or < -180f)
        {
            yawDiff -= 360f * Mathf.Sign(yawDiff);
        }
        
        yawDiff = Mathf.Clamp(yawDiff, -yawOffsetMax, yawOffsetMax);
        centralPitch = Mathf.Clamp(targetPitch - centralPitch, -pitchOffsetMax, pitchOffsetMax) + centralPitch;
        
        return new CameraConfiguration()
        {
            yaw = centralYaw + yawDiff,
            roll = this.roll,
            pitch = centralPitch,
            fov = this.fov,
            distance = 0f,
            pivot = transform.position
        };
    }
    
    
}
