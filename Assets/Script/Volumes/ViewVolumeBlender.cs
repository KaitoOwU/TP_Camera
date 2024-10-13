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
        List<AViewVolume> allVolumes = new();
        foreach (AViewVolume viewVolume in activeViewVolumes)
        {
            viewVolume.view.weight = 0;
            allVolumes.Add(viewVolume);
        }

        allVolumes = allVolumes.OrderBy(x => x.weight).ThenBy(x => x.Uid).ToList();
        float totalWeight = 1f;
        foreach (AViewVolume volume in allVolumes)
        {
            volume.view.weight = volume.ComputeSelfWeight(totalWeight);
            totalWeight -= volume.view.weight;
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
