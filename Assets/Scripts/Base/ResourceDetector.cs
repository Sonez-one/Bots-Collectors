using System.Collections.Generic;
using UnityEngine;

public class ResourceDetector : MonoBehaviour
{
    [SerializeField] private Vector3 _overlapBoxSize;
    [SerializeField] private LayerMask _layerMask;

    private void Start()
    {
        _overlapBoxSize = new Vector3(50f, 50f, 50f);
    }

    public List<Resource> GetAvailableResources()
    {
        List<Resource> availableResources = ScanForResorces();

        if (availableResources.Count > 0)
        {
            return availableResources;
        }

        return null;
    }

    private List<Resource> ScanForResorces()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, _overlapBoxSize, Quaternion.identity, _layerMask);

        List<Resource> availableResources = new List<Resource>();

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out Resource resource))
            {
                availableResources.Add(resource);
            }
        }

        return availableResources;
    }
}