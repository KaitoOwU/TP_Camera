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
    [Range(0f, 1f)] public float weight;
    public AView view;
    public int Uid { get; private set; }
    public bool isCutOnSwitch;
    
    protected bool IsActive { get; private set; }

    private void Awake()
    {
        Uid = nextUid;
        ++nextUid;
    }

    public virtual float ComputeSelfWeight(float remainingWeight) => remainingWeight * weight;

    protected void SetActive(bool active)
    {
        IsActive = active;
        
        if (active)
        {
            ViewVolumeBlender.Instance.AddVolume(this);
        }
        else
        {
            ViewVolumeBlender.Instance.RemoveVolume(this);
        }

        if (!isCutOnSwitch)
            ViewVolumeBlender.Instance.Update();
        else
            CameraController.Instance.Cut();
    }
}
