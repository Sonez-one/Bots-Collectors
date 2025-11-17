using System;
using System.Collections;
using UnityEngine;

public class BotDistanceChecker : MonoBehaviour
{
    [SerializeField] private float _pickupDistance = 1f;

    private ITargetable _target;
    private Coroutine _checkRoutine;

    public event Action ResourceReached;

    public void SetTarget(ITargetable target)
    {
        _target = target;

        if (_checkRoutine != null)
        {
            StopCoroutine(Check());
        }

        _checkRoutine = StartCoroutine(Check());
    }

    private IEnumerator Check()
    {
        while (_target != null)
        {
            if (Vector3Extensions.IsEnoughClose(transform.position, _target.Position, _pickupDistance))
            {
                if (_target is Resource)
                {
                    ResourceReached?.Invoke();
                }

                _target = null;
            }

            yield return null;
        }
    }
}