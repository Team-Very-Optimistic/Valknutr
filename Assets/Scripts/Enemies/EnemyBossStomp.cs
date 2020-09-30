using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossStomp : MonoBehaviour
{
    private float colliderActiveTime = 0.05f;
    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
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
            Destroy(this);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            var damageScript = GetComponent<Damage>();
            damageScript.DealDamage(other);
        }
    }
}
