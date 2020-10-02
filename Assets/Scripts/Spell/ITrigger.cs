using System;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    // Type of function that handles Start events
    public delegate void EventHandler<Collider>(Collider other);
 
    // Event used to forward the Start event
    public event EventHandler<Collider> OnTriggerEnterEvent = other => {};
    
    
    public abstract void TriggerEvent(Collider other);

    public void Start()
    {
        OnTriggerEnterEvent += TriggerEvent();
    }

    public void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent(other);
    }
}