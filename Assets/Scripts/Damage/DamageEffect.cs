using System;
using UnityEngine;

[Serializable]
public abstract class DamageEffect
{
    public abstract void CastDamageEffect(Collider other, float damage);
    
}