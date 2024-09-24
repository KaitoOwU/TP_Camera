using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }   
    public Camera MainCamera { get; private set; }
    public CameraConfiguration Configuration { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Update()
    {
        ApplyConfiguration();
    }

    private void ApplyConfiguration()
    {
        MainCamera.transform.rotation = Configuration.GetRotation();
        MainCamera.transform.position = Configuration.GetPosition();
    }
    
}
