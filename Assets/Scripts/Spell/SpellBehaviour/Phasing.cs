using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerEventHandler), typeof(Damage))]
public class Phasing : MonoBehaviour
{
    public float _damage;
    public int _phaseNum = 2;
    public List<TriggerEventHandler> triggers;
    public void Start()
    {
        var trig = GetComponents<TriggerEventHandler>();
        triggers = new List<TriggerEventHandler>();
        foreach (var t in trig)
        {
            t.OverrideEvent(Trigger);
            triggers.Add(t);
        }
    }
    
    //public bool CanTrigger { get; set; }
    public void Trigger(Collider other)
    {
        var damageScript = GetComponent<Damage>();
        damageScript.SetDamage(_damage);   
        damageScript.DealDamage(other);
        _phaseNum--;

        if (_phaseNum < 0)
        {
            foreach (var t in triggers)
            {
                t.RemoveOverride(Trigger);
            }
        }
    }
}