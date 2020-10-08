

using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A normal damage trap that uses collider
/// </summary>
[RequireComponent(typeof(Collider), typeof(Damage))]
public class Trap : MonoBehaviour
{
    public float damage = 1.5f;
    public float knockbackForce = 500f;

    [Header("Trap Effects")]
    public string trapSound;
    public string trapEffect;
    private float radius;
    private Damage _damage;
    protected virtual void Start()
    {
        _damage = GetComponent<Damage>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var transformPosition = transform.position;
        AudioManager.PlaySoundAtPosition(trapSound, transformPosition, damage * 0.05f, Random.Range(0.8f, 1.2f));
        

        if (other.GetComponent<EnemyBehaviourBase>() != null)
        {
            //Enable knockback on enemies
            other.GetComponent<EnemyBehaviourBase>().EnableKnockback(true);
        }

        var direction = (transformPosition - other.ClosestPoint(transformPosition)).normalized * knockbackForce * damage;
        other.attachedRigidbody.AddForce(direction);
        
        EffectManager.PlayEffectAtPosition(trapEffect, transformPosition);
        _damage.SetDamage(damage);
        _damage.DealDamage(other);
    }
}
