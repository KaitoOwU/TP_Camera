using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalViewVolume : AViewVolume
{
    private void Start()
    {
        SetActive(true);
    }
    
    private void OnGUI()
    {
        foreach (var volume in ViewVolumeBlender.Instance.activeViewVolumes)
        {
            GUILayout.Label(volume.name);
        }
    }
}
