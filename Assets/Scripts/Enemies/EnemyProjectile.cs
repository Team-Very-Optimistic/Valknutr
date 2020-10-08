using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyProjectile : MonoBehaviour
{
    public float _damage = 1;
    public Vector3 direction;
    public float speed;
    public float timeToExpire = 20f;

    public void Launch(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        gameObject.GetComponent<Rigidbody>().velocity = direction * speed;
        Destroy(gameObject, timeToExpire);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (other.gameObject.GetComponent<PlayerHealth>() != null && other.GetType() == typeof(CapsuleCollider))
            {
                var damageScript = GetComponent<Damage>();
                damageScript.SetDamage(_damage);
                damageScript.DealDamage(other);
                Destroy(gameObject);
            }
        }
        else if (!other.gameObject.tag.Equals("Enemy") && !other.gameObject.tag.Equals("Projectile"))
        {
            Debug.Log(other.gameObject);
            Destroy(gameObject);
        }


    }
}