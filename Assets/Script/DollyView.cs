using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DollyView : AView
{
    [Range(-180f, 180f)] public float roll;
    [Min(0f)] public float distance;
    [Range(0.0f, 180f)] public float fov;

    [SerializeField] private bool IsAuto;
    [SerializeField] private Rail rail;
    [SerializeField] private GameObject target;
    [SerializeField] private float _speed;
    private float _pitch;
    private float _yaw;
    private Vector3 _pivot;
    private float distanceOnRail = 0;

    private void Update()
    {
        distanceOnRail += Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
    }

    public override CameraConfiguration GetConfiguration()
    {
        if (IsAuto)
            _pivot = rail.GetClosestPointOnRail(target.transform.position);
        else
            _pivot = rail.GetPosition(distanceOnRail);

        Vector3 targetDir = target.transform.position - _pivot;

        float targetYaw = Mathf.Atan2(targetDir.normalized.x, targetDir.normalized.z) * Mathf.Rad2Deg;
        float targetPitch = -Mathf.Asin(targetDir.normalized.y) * Mathf.Rad2Deg;

        _yaw = targetYaw;
        _pitch = targetPitch;

        return new CameraConfiguration()
        {
            yaw = targetYaw,
            roll = this.roll,
            pitch = targetPitch,
            fov = this.fov,
            distance = 0f,
            pivot = _pivot,
        };
    }
}
