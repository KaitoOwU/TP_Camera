using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedView : AView
{
    [Range(0.0f, 360f)] public float yaw;
    [Range(-90f, 90f)] public float pitch;
    [Range(-180f, 180f)] public float roll;
    [Range(0.0f, 180f)] public float fov;
    
    public override CameraConfiguration GetConfiguration()
    {
        return new CameraConfiguration()
        {
            yaw = this.yaw,
            roll = this.roll,
            pitch = this.pitch,
            fov = this.fov,
            distance = 0f,
            pivot = transform.position
        };
    }
}
