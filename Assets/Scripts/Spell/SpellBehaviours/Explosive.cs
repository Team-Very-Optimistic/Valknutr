using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.Effects;

[RequireComponent(typeof(Damage))]
public class Explosive : TriggerEventHandler {
    public float _damage = 10;

    public float timeToExpire = 8f;
    public float radius = 5f;
    public float power = 10f;
    public float fuseTime = 0f;
    
    [SerializeField]
    private bool _explode;
    public void Launch(Vector3 direction, float speed)
    {
        gameObject.GetComponentElseAddIt<Rigidbody>().velocity = direction.normalized * speed;
        //StartCoroutine(Explode(timeToExpire));
        Destroy(gameObject, timeToExpire);
    }

    public override void TriggerEvent(Collider other)
    {
        Detonate(fuseTime);
    }
    
    public void Detonate(float time = 0)
    {
        if (_explode) return;
        _explode = true;
        StopAllCoroutines();
        StartCoroutine(Explode(time));
    }

    IEnumerator Explode(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 explosionPos = transform.position;
        var damageScript = GetComponent<Damage>();
        damageScript.SetDamage(_damage);   
        
        var colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider hit in colliders)
        {
            if(hit.gameObject != GameManager.Instance._player)
                damageScript.DealDamage(hit);
        }
        
        AudioManager.PlaySoundAtPosition("explosion", transform.position);
        ScreenShakeManager.Instance.ScreenShake(0.5f, 0.8f * power / 2000f);
        //ONly works for one prefab
        GameObject o = null;
        if(gameObject.transform.childCount < 1)
        {
            o = gameObject;
        }
        else
        {
            o = gameObject.transform.GetChild(1).gameObject;
        }
        var explosionPhysicsForce = gameObject.GetComponentElseAddIt<ExplosionPhysicsForce>();
        explosionPhysicsForce.explosionForce = power;
        explosionPhysicsForce.explosionRadius = radius;
        o.transform.localScale *= radius;
        o.SetActive(true);
        o.transform.SetParent(null);
        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}