using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AView : MonoBehaviour
{

    [Min(1f)] public float weight;
    public bool isActiveAtStart;

    public abstract CameraConfiguration GetConfiguration();

    private void Start()
    {
        if(isActiveAtStart)
            SetActive(true);
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            CameraController.Instance.AddView(this);
        }
        else
        {
            CameraController.Instance.RemoveView(this);
        }
    }
    
    private void OnDrawGizmos()
    {
        GetConfiguration().DrawGizmos(Color.blue);
    }

}
