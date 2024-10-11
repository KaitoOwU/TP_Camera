using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AViewVolume : MonoBehaviour
{
    public static int nextUid = 0;
    
    [Min(0)] public int priority = 0;
    public AView view;
    public int Uid { get; private set; }
    public bool isCutOnSwitch;
    
    protected bool IsActive { get; private set; }

    private void Awake()
    {
        Uid = nextUid;
        ++nextUid;
    }

    public virtual float ComputeSelfWeight() => 1.0f;

    protected void SetActive(bool active)
    {
        IsActive = active;

        if (isCutOnSwitch)
            ViewVolumeBlender.Instance.Update();
        else
            CameraController.Instance.Cut();

        if (active)
        {
            ViewVolumeBlender.Instance.AddVolume(this);
        }
        else
        {
            ViewVolumeBlender.Instance.RemoveVolume(this);
        }
    }
}
