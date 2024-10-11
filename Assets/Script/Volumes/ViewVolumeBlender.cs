using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    public static ViewVolumeBlender Instance { get; private set; }
    
    public List<AViewVolume> activeViewVolumes = new();
    public Dictionary<AView, List<AViewVolume>> volumePerViews = new();

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public void Update()
    {
        foreach (AView view in volumePerViews.Keys)
        {
            view.weight = 0;
        }

        activeViewVolumes = activeViewVolumes.OrderBy(v => v.priority).ThenBy(v => v.Uid).ToList();
        float remainingWeight = 1.0f;
        foreach (var volume in activeViewVolumes)
        {
            float weight = Mathf.Clamp01(volume.ComputeSelfWeight());
            remainingWeight -= weight;
            foreach (var view in CameraController.Instance.ActiveViews)
            {
                view.weight *= remainingWeight;
            }
            volume.view.weight += weight;
        }
    }

    public void AddVolume(AViewVolume volume)
    {
        activeViewVolumes.Add(volume);

        if (!volumePerViews.ContainsKey(volume.view))
        {
            volume.view.SetActive(true);
            volumePerViews[volume.view] = new List<AViewVolume>() { volume };
        }
        else
        {
            volumePerViews[volume.view].Add(volume);
        }
    }

    public void RemoveVolume(AViewVolume volume)
    {
        activeViewVolumes.Remove(volume);
        volumePerViews[volume.view].Remove(volume);
        if (volumePerViews[volume.view].Count == 0)
        {
            volumePerViews.Remove(volume.view);
            volume.view.SetActive(false);
        }
    }
}
