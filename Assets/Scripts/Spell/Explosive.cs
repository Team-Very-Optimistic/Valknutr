using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Damage))]
public class Explosive : MonoBehaviour {
    public float _damage = 1;
    public Vector3 direction;
    public float speed;
    public float timeToExpire = 20f;
    public float radius = 20f;
    public float power = 10f;
    private bool _explode;

    public void Launch(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        gameObject.GetComponent<Rigidbody>().velocity = direction * speed;
        StartCoroutine(Explode(5f));
    }
    
    public void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.tag.Equals("Player")  || other.gameObject.tag.Equals("Projectile"))
        // {
        //     return;
        // }
        if (_explode) return;
        StopAllCoroutines();
        _explode = true;
        StartCoroutine(Explode(0));
    }

    IEnumerator Explode(float time)
    {
        yield return new WaitForSeconds(time);
        var damageScript = GetComponent<Damage>();
        damageScript.SetDamage(_damage);   
        
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        Debug.Log(colliders.Length);
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            damageScript.DealDamage(hit);
            if (rb != null)
            {
                Debug.Log(hit.name);
                rb.AddExplosionForce(power* 999999, explosionPos, radius, 3.0F);
            }
        }
        Debug.Log("Explode");
        Destroy(gameObject);
    }
    
}