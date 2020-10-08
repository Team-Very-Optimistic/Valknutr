using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossStomp : MonoBehaviour
{
    private float colliderActiveTime = 0.05f;
    private float elapsedTime;
    private float prefabActiveTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, prefabActiveTime);
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
            GetComponent<Collider>().enabled = false;
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
