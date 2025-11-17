using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour, ITargetable
{
    public event Action<Resource> IsCollected;

    public Vector3 Position => transform.position;

    public void SetCollected()
    {
        IsCollected.Invoke(this);
    }

    public void ResetParameters()
    {
        transform.SetParent(null);
    }
}