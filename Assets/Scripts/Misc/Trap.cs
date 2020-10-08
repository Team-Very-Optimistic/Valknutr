

using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A normal damage trap that uses collider
/// </summary>
[RequireComponent(typeof(Collider), typeof(Damage))]
public class Trap : MonoBehaviour
{
    public float damage = 1.5f;
    [Range(0.05f, 10f)]
    public float damageInterval = 0.3f;
    public float knockbackForce = 500f;

    [Header("Trap Effects")]
    public string trapSound;
    public string trapEffect;
    private float radius;
    private Damage _damage;
    private float time = 0;
    protected virtual void Start()
    {
        _damage = GetComponent<Damage>();
    }

    protected virtual void TriggerEvent(Collider other)
    {
        var transformPosition = transform.position;
        AudioManager.PlaySoundAtPosition(trapSound, transformPosition, damage * 0.05f, Random.Range(0.8f, 1.2f));
        

        if (other.GetComponent<EnemyBehaviourBase>() != null)
        {
            //Enable knockback on enemies
            other.GetComponent<EnemyBehaviourBase>().EnableKnockback(true);
        }

        if (other.attachedRigidbody != null)
        {
            var direction = (other.ClosestPoint(transformPosition) - transformPosition);
            direction.y = 0;
            direction = direction.normalized * knockbackForce * damage;
            other.attachedRigidbody.AddForce(direction);
        }

        EffectManager.PlayEffectAtPosition(trapEffect, transformPosition);
        _damage.SetDamage(damage);
        _damage.DealDamage(other);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged") || other.CompareTag("Fire")) return;
        float diff = Time.timeSinceLevelLoad - time;
        if (damageInterval < diff)
        {
            TriggerEvent(other);
            time = Time.timeSinceLevelLoad;
            return;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Untagged") || other.CompareTag("Fire")) return;
        float diff = Time.timeSinceLevelLoad - time;

        if (damageInterval< diff)
        {
            TriggerEvent(other);
            time = Time.timeSinceLevelLoad;
            return;
        }
        
    }
}
