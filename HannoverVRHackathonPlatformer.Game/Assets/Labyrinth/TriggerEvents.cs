using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    private Collider _collider;

    public ColliderEvent Enter;
    public ColliderEvent Exit;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enter.Invoke(_collider, other);
    }

    private void OnTriggerExit(Collider other)
    {
        Exit.Invoke(_collider, other);
    }
}

[Serializable]
public class ColliderEvent : UnityEvent<Collider, Collider> { }