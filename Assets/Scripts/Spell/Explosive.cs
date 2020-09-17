using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.Effects;

[RequireComponent(typeof(Damage))]
public class Explosive : MonoBehaviour {
    public float _damage = 10;
    public Vector3 direction;
    public float speed;
    public float timeToExpire = 20f;
    public float radius = 7f;
    public float power = 10f;
    [SerializeField]
    private bool _explode;

    public void Launch(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        gameObject.GetComponent<Rigidbody>().velocity = direction * speed;
        Destroy(gameObject, timeToExpire);
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
        StartCoroutine(Explode(1.8f));
    }

    IEnumerator Explode(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 explosionPos = transform.position;
        var damageScript = GetComponent<Damage>();
        damageScript.SetDamage(_damage);   
        
        Collider[] colliders = new Collider[10];
        var size = Physics.OverlapSphereNonAlloc(explosionPos, radius, colliders);
        Debug.Log(size);

        for (int i = 0; i < size; i++)
        {
            Collider hit = colliders[i];
            damageScript.DealDamage(hit);
        }
        
        //ONly works for one prefab
        var o = gameObject.transform.GetChild(1).gameObject;
        var explosionPhysicsForce = o.GetComponent<ExplosionPhysicsForce>();
        explosionPhysicsForce.explosionForce = power;
        explosionPhysicsForce.explosionRadius = radius;
        o.transform.localScale = Vector3.one * radius / 7f;
        o.SetActive(true);
        o.transform.SetParent(null);
        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}