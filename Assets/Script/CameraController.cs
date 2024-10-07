using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }   
    public Camera MainCamera { get; private set; }

    [SerializeField] float _transitionSpeed;
    private CameraConfiguration _currentConfiguration, _targetConfiguration;
    private List<AView> _activeViews = new List<AView>();
    private float dampingT;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }
    private void Start()
    {
        _targetConfiguration = ComputeAverage();
        _currentConfiguration = _targetConfiguration;        
    }

    private void Update()
    {
        _targetConfiguration = ComputeAverage();

        ApplyConfiguration();
    }

    private void ApplyConfiguration()
    {
        if (_transitionSpeed * Time.deltaTime < 1)
            dampingT = /*(_targetConfiguration - _currentConfiguration) * */_transitionSpeed * Time.deltaTime;
        else
            dampingT = 1;

        _currentConfiguration = CameraConfiguration.LerpConfig(_currentConfiguration, _targetConfiguration, dampingT);

        MainCamera.transform.rotation = this._currentConfiguration.GetRotation();
        MainCamera.transform.position = this._currentConfiguration.GetPosition();        
    }

    public void AddView(AView view)
    {
        _activeViews.Add(view);
    }

    public void RemoveView(AView view)
    {
        _activeViews.Remove(view);
    }

    public CameraConfiguration ComputeAverage()
    {
        CameraConfiguration newConfig = new();
        float weight = 0f;

        foreach (AView view in _activeViews)
        {
            weight += view.weight;
            newConfig.pitch += view.GetConfiguration().pitch * view.weight;
            newConfig.roll += view.GetConfiguration().roll * view.weight;
            newConfig.distance += view.GetConfiguration().distance * view.weight;
            newConfig.fov += view.GetConfiguration().fov * view.weight;
            newConfig.pivot += view.GetConfiguration().pivot * view.weight;
        }

        newConfig.yaw = ComputeAverageYaw();
        newConfig.pitch /= weight;
        newConfig.roll /= weight;
        newConfig.distance /= weight;
        newConfig.fov /= weight;
        newConfig.pivot /= weight;

        return newConfig;
    }
    
    private float ComputeAverageYaw()
    {
        Vector2 sum = Vector2.zero;
        foreach (AView view in _activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad),
                Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }

    private void OnDrawGizmos()
    {
        _currentConfiguration.DrawGizmos(Color.red);
        _targetConfiguration.DrawGizmos(Color.cyan);
    }
}
