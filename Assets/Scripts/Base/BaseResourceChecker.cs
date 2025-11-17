using System.Collections.Generic;
using UnityEngine;

public class BaseResourceChecker : MonoBehaviour
{
    private List<Resource> _targetedResources;

    private void Awake()
    {
        _targetedResources = new List<Resource>();
    }

    public void AddTargetedResource(Resource resource)
    {
        _targetedResources.Add(resource);
    }

    public void RemoveTargetedResource(Resource resource)
    {
        _targetedResources.Remove(resource);
    }

    public bool CheckTargetedResource(Resource resource)
    {
        if (_targetedResources.Contains(resource))
        {
            return true;
        }

        return false;
    }
}