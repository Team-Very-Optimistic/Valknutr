using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float timeToExpire = 50f;

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
        Destroy(gameObject);
    }
}