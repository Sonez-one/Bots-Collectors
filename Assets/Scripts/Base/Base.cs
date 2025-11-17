using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour, ITargetable
{
    private readonly float _delay = 2f;

    [SerializeField] private ResourceDetector _resourceDetector;
    [SerializeField] private BaseResourceCollector _resourceCollector;
    [SerializeField] private BaseResourceChecker _resourceChecker;
    [SerializeField] private List<Bot> _bots;

    private List<Resource> _deliveringResources;
    private Coroutine _coroutine;

    private int _goldValue = 0;
    private int _ironValue = 0;
    private int _woodValue = 0;

    public event Action<int> GoldValueChanged;
    public event Action<int> IronValueChanged;
    public event Action<int> WoodValueChanged;

    public Vector3 Position => transform.position;

    private void OnEnable()
    {
        _resourceCollector.ResourceDetected += VerifyDeliveredResource;
    }

    private void OnDisable()
    {
        _resourceCollector.ResourceDetected -= VerifyDeliveredResource;
    }

    private void Start()
    {
        _deliveringResources = new List<Resource>();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(GiveWorkToBots());
    }

    private void VerifyDeliveredResource(Resource resource)
    {
        foreach (Resource deliveringResource in _deliveringResources)
        {
            if (deliveringResource == resource)
            {
                CollectResource(resource);

                break;
            }
        }
    }

    private void CollectResource(Resource newResource)
    {
        _deliveringResources = _deliveringResources.Where(resource => resource != newResource).ToList();

        _resourceChecker.RemoveTargetedResource(newResource);
        newResource.SetCollected();

        if (newResource is Gold)
        {
            _goldValue++;

            GoldValueChanged.Invoke(_goldValue);
        }
        else if (newResource is Iron)
        {
            _ironValue++;

            IronValueChanged.Invoke(_ironValue);
        }
        else if (newResource is Wood)
        {
            _woodValue++;

            WoodValueChanged.Invoke(_woodValue);
        }
    }

    private List<Bot> FindFreeBots()
    {
        List<Bot> freeBots = new List<Bot>();

        foreach (Bot bot in _bots)
        {
            if (bot.TargetedResource == null)
            {
                freeBots.Add(bot);
            }
        }

        return freeBots;
    }

    private void GiveCollectingOrder(Bot bot, Resource target)
    {
        bot.SetTarget(target);
    }

    private IEnumerator GiveWorkToBots()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            yield return wait;

            List<Bot> freeBots = FindFreeBots();

            if (freeBots.Count > 0)
            {
                AssignResources(freeBots);
            }
        }
    }

    private void AssignResources(List<Bot> freeBots)
    {
        List<Resource> availableResources = _resourceDetector.GetAvailableResources();

        if (availableResources != null)
        {
            availableResources = SortResources(availableResources);

            for (int i = 0; i < freeBots.Count; i++)
            {
                if (availableResources.Count > i)
                {
                    GiveCollectingOrder(freeBots[i], availableResources[i]);
                    _resourceChecker.AddTargetedResource(availableResources[i]);
                    _deliveringResources.Add(availableResources[i]);
                }
            }
        }
    }

    private List<Resource> SortResources(List<Resource> availableResources)
    {
        availableResources = availableResources.Where(resource =>
        _resourceChecker.CheckTargetedResource(resource) == false).ToList();

        availableResources = availableResources.OrderBy(resource =>
        Vector3Extensions.SqrDistance(transform.position, resource.transform.position)).ToList();

        return availableResources;
    }
}