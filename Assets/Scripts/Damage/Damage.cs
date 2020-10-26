using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    protected float damage = 1;

    public List<DamageEffect> damageEffects;

    public virtual bool DealDamage(Collider other)
    {
        if (other.GetComponent<HealthScript>() != null)
        {
            other.GetComponent<HealthScript>().ApplyDamage(damage);
            foreach (var damageEffect in damageEffects)
            {
                damageEffect.CastDamageEffect(other, damage);
            }
            return true;
        }

        return false;
    }

    public void AddDamageEffect(DamageEffect damageEffect)
    {
        if(!damageEffects.Contains(damageEffect))
            damageEffects.Add(damageEffect);
    }
    public void RemoveDamageEffect(DamageEffect damageEffect)
    {
        damageEffects.Remove(damageEffect);
    }

    public void Start()
    {
        damageEffects = new List<DamageEffect>();
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