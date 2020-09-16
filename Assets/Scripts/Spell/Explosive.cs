using UnityEngine;

[RequireComponent(typeof(Damage))]
public class Explosive : MonoBehaviour {
    public float _damage = 1;
    public Vector3 direction;
    public float speed;
    public float timeToExpire = 20f;
    public float radius = 20f;
    public float power = 10f;
    
    public void Launch(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        gameObject.GetComponent<Rigidbody>().velocity = direction * speed;
        Destroy(gameObject, timeToExpire);    
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")  || other.gameObject.tag.Equals("Projectile"))
        {
            return;
        }
        var damageScript = GetComponent<Damage>();
        damageScript.SetDamage(_damage);   
        
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            damageScript.DealDamage(hit);
            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
        Destroy(gameObject);
    }
}