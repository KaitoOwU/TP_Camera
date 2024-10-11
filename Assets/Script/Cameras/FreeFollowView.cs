using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{
    [Range(-90f, 90f)] public float[] pitch = new float[3];
    [Range(-180f, 180f)] public float[] roll = new float[3];
    [Range(0.0f, 180f)] public float[] fov = new float[3];
    
    [Range(0.0f, 360f)] public float yaw;
    public float yawSpeed;

    public Transform target;
    public Curve curve;
    [Range(0f, 1f)] float _curvePosition;
    public float curveSpeed;

    public Matrix4x4 CurveToWorldMatrix => Matrix4x4.TRS(
        target.position,
        Quaternion.Euler(0, yaw, 0),
        Vector3.one);

    private void Update()
    {
        yaw = Mathf.Repeat(yaw + (yawSpeed * Time.deltaTime * Input.GetAxis("Horizontal")), 360f);
        _curvePosition += curveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
    }
    
    public override CameraConfiguration GetConfiguration()
    {
        float finalPitch = _curvePosition < 0.5f ? Mathf.Lerp(pitch[0], pitch[1], _curvePosition * 2) : Mathf.Lerp(pitch[1], pitch[2], (_curvePosition - 0.5f) * 2);
        float finalRoll = _curvePosition < 0.5f ? Mathf.Lerp(roll[0], roll[1], _curvePosition * 2) : Mathf.Lerp(roll[1], roll[2], (_curvePosition - 0.5f) * 2);
        float finalFov = _curvePosition < 0.5f ? Mathf.Lerp(fov[0], fov[1], _curvePosition * 2) : Mathf.Lerp(fov[1], fov[2], (_curvePosition - 0.5f) * 2);
        
        return new CameraConfiguration()
        {
            yaw = yaw,
            pitch = finalPitch,
            roll = finalRoll,
            fov = finalFov,
            distance = 0f,
            pivot = curve.GetPosition(_curvePosition, CurveToWorldMatrix)
        };
    }

    private void OnDrawGizmos()
    {
        curve.DrawGizmos(Color.blue, transform.localToWorldMatrix);
    }
}
