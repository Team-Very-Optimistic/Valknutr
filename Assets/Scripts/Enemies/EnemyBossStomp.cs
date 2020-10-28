using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossStomp : MonoBehaviour
{
    private float colliderActiveTime = 0.05f;
    private float elapsedTime;
    private float prefabActiveTime = 5.0f;

    private Collider _cOllider;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, prefabActiveTime);
        _cOllider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(elapsedTime < colliderActiveTime)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            _cOllider.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if(other.gameObject.GetComponent<PlayerHealth>() != null && other.GetType() == typeof(CapsuleCollider))
            {
                var damageScript = GetComponent<Damage>();
                damageScript.DealDamage(other);
            }
        }
    }
}
