using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public Transform target;
    public float outerRadius, innerRadius;

    private float _distance;

    private void Update()
    {
        _distance = (target.position - transform.position).magnitude;
        if (_distance <= outerRadius && !IsActive)
        {
            SetActive(true);
        } else if (_distance > outerRadius && IsActive)
        {
            SetActive(false);   
        }
    }

    public override float ComputeSelfWeight(float remainingWeight)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= innerRadius) return base.ComputeSelfWeight(remainingWeight);
        else if (distance >= outerRadius) return 0f;
        else
        {
            distance -= innerRadius;
            float delta = outerRadius - innerRadius;
            return (delta - distance) / delta * base.ComputeSelfWeight(remainingWeight);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
    
    private void OnValidate()
    {
        if (innerRadius > outerRadius)
        {
            innerRadius = outerRadius;
        }
    }
}
