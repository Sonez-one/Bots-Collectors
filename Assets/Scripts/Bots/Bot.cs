using UnityEngine;

[RequireComponent(typeof(BotMover))]
[RequireComponent(typeof(BotDistanceChecker))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Base _base;

    private BotMover _mover;
    private BotDistanceChecker _distanceChecker;
   
    public Resource TargetedResource { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<BotMover>();
        _distanceChecker = GetComponent<BotDistanceChecker>();
    }

    private void OnEnable()
    {
        _distanceChecker.ResourceReached += PickUpResource;
    }

    private void OnDisable()
    {
        _distanceChecker.ResourceReached -= PickUpResource;
    }

    private void Update()
    {
        if (TargetedResource == null || TargetedResource.isActiveAndEnabled == false)
        {
            TargetedResource = null;

            _mover.ClearTarget();
        }
    }

    public void SetTarget(ITargetable target)
    {
        _mover.SetTarget(target);
        _distanceChecker.SetTarget(target);

        if (target is Resource)
        {
            TargetedResource = (Resource)target;
        }
    }

    private void PickUpResource()
    {
        TargetedResource.transform.SetParent(transform);

        TargetedResource.transform.localPosition = Vector3.zero;
        TargetedResource.transform.localEulerAngles = Vector3.zero;

        _mover.SetTarget(_base);
    }
}