using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : TriggerEventHandler
{
    public float _damage = 3;
    public Vector3 direction;
    public float speed;
    public float timeToExpire = 20f;
    private float explosionRadius = 2f;
    private float explosionForce = 500f;

    public void Launch(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        gameObject.GetComponent<Rigidbody>().velocity = direction * speed;
        AudioManager.PlaySoundAtPosition("projectileLaunch", transform.position);
        Destroy(gameObject, timeToExpire);
    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     
    //     TriggerEvent(other);
    // }
    
    public override void TriggerEvent(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")  || other.gameObject.tag.Equals("Projectile"))
        {
            return;
        }
        AudioManager.PlaySoundAtPosition("projectileHit", transform.position);
        
        var cols = Physics.OverlapSphere(transform.position, explosionRadius);
        
        
        foreach (var col in cols)
        {
            if (!col.CompareTag("Player") && col.attachedRigidbody != null )
            {
                if (col.gameObject.GetComponent<EnemyBehaviourBase>() != null)
                {
                    //Enable knockback on enemies
                    col.gameObject.GetComponent<EnemyBehaviourBase>().EnableKnockback(true);
                }

                col.attachedRigidbody.AddExplosionForce(explosionForce * _damage, transform.position - direction, explosionRadius, 0.0f);
            }
        }

        EffectManager.PlayEffectAtPosition("projectileHit", transform.position);
        var damageScript = GetComponent<Damage>();
        damageScript.SetDamage(_damage);
        damageScript.DealDamage(other);
        Destroy(gameObject);
        
    }
}