using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour, ITrigger
{
    public float _damage = 1;
    public Vector3 direction;
    public float speed;
    public float timeToExpire = 20f;
    private float explosionRadius = 2f;
    private float explosionForce = 1f;

    public void Launch(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        gameObject.GetComponent<Rigidbody>().velocity = direction * speed;
        AudioManager.PlaySoundAtPosition("projectileLaunch", transform.position);
        Destroy(gameObject, timeToExpire);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")  || other.gameObject.tag.Equals("Projectile"))
        {
            return;
        }
        Trigger(other);
    }


    public void Trigger(Collider other)
    {
        AudioManager.PlaySoundAtPosition("projectileHit", transform.position);
        
        var cols = Physics.OverlapSphere(transform.position, explosionRadius);
        
        
        foreach (var col in cols)
        {
            if (!col.CompareTag("Player") && col.attachedRigidbody != null )
            {
                col.attachedRigidbody.AddExplosionForce(explosionForce * _damage, transform.position, explosionRadius, 1, ForceMode.Impulse);
            }
        }

        EffectManager.PlayEffectAtPosition("projectileHit", transform.position);
        var damageScript = GetComponent<Damage>();
        damageScript.SetDamage(_damage);
        damageScript.DealDamage(other);
        Destroy(gameObject);
        
    }
}