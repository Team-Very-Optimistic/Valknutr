using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Damage))]
public class Phasing : MonoBehaviour, ITrigger
{
    public float _damage;
    public int _phaseNum = 2;
    public List<MonoBehaviour> triggers;
    public void Start()
    {
        var comps = GetComponents<MonoBehaviour>();
        triggers = new List<MonoBehaviour>();
        foreach (var comp in comps)
        {
            if (comp != this && comp is ITrigger)
            {
                triggers.Add(comp);
                comp.enabled = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Trigger(other);
    }

    public bool CanTrigger { get; set; }
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
                t.enabled = true;
            }
        }
    }
}