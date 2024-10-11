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
        if (_distance <= outerRadius && !IsActive)
        {
            SetActive(true);
        } else if (_distance > outerRadius && IsActive)
        {
            SetActive(false);   
        }
    }

    public override float ComputeSelfWeight()
    {
        return Mathf.Lerp(0f, 1f, (target.position - transform.position).normalized.magnitude);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, outerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position, innerRadius);
    }
    
    //SECURITY
    private void OnValidate()
    {
        if (innerRadius > outerRadius)
        {
            innerRadius = outerRadius;
        }
    }
}
