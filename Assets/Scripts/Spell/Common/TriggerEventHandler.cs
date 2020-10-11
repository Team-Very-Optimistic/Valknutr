using UnityEngine;


public abstract class TriggerEventHandler : MonoBehaviour
{
    public delegate void EventHandler<Collider>(Collider other);
 
    // Event used to forward the trigger event
    public event EventHandler<Collider> OnTriggerEnterEvent = other => {};
    //List<EventHandler<Collider>> delegates = new List<EventHandler<Collider>>();
    private EventHandler<Collider> savedDelegate;

    private Collider[] _collidersInteractedWith;
    
    public abstract void TriggerEvent(Collider other);

    protected virtual void Start()
    {
        savedDelegate = TriggerEvent;
        //delegates.Add(TriggerEvent);
        OnTriggerEnterEvent += TriggerEvent;
    }

    public void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent(other);
    }

    /// <summary>
    /// Overrides the ontriggerenter event with another delegate instead.
    /// </summary>
    public void OverrideEvent(EventHandler<Collider> trigger)
    {
        OnTriggerEnterEvent -= savedDelegate;
        OnTriggerEnterEvent += trigger;
    }
    
    public void AddEvent(EventHandler<Collider> trigger)
    {
        OnTriggerEnterEvent += trigger;
    }
    public void RemoveEvent(EventHandler<Collider> trigger)
    {
        OnTriggerEnterEvent -= trigger;
    }

    /// <summary>
    /// Returns normal functionality
    /// </summary>
    public void RemoveOverride(EventHandler<Collider> trigger)
    {
        OnTriggerEnterEvent -= trigger;
        OnTriggerEnterEvent += savedDelegate;
    }
}