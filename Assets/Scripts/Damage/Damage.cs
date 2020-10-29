using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
{
    [SerializeField]
    protected float damage = 1;


    public UnityEvent<Collider, float> _damageCallback;
    
    [SerializeField]
    private List<DamageEffect> damageEffects;

    public virtual bool DealDamage(Collider other)
    {
        if (other.GetComponent<HealthScript>() != null)
        {
            other.GetComponent<HealthScript>().ApplyDamage(damage);
            _damageCallback?.Invoke(other, damage);
            return true;
        }

        return false;
    }

    public void AddDamageEffect(DamageEffect damageEffect)
    {
        if(damageEffect == null)
            return;
        if (!damageEffects.Contains(damageEffect))
        {
            damageEffects.Add(damageEffect);
            _damageCallback.AddListener(damageEffect.CastDamageEffect);
        }
    }
    public void RemoveDamageEffect(DamageEffect damageEffect)
    {
        damageEffects.Remove(damageEffect);
        _damageCallback.RemoveListener(damageEffect.CastDamageEffect);
    }

    public void Start()
    {
        damageEffects = new List<DamageEffect>();
        if (_damageCallback == null)
            _damageCallback = new UnityEvent<Collider, float>();

    }

    //Getters/Setters
    public float GetDamage()
    {
        return damage;
    }    

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

}