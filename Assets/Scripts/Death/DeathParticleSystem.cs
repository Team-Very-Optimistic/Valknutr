using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticleSystem : MonoBehaviour
{
    private GameObject ragdollChildGameObject;
    private float ragdollAliveTime;
    private bool triggerStopSystem = false;

    public void Start()
    {
        GetComponent<ParticleSystem>().Play();
    }

    public void Update()
    {
        if(ragdollChildGameObject != null)
        {
            this.transform.position = ragdollChildGameObject.transform.position;
        }
    }

    public void SetRagdollAliveTime(float ragdollAliveTime)
    {
        this.ragdollAliveTime = ragdollAliveTime;
        Invoke(nameof(Die), ragdollAliveTime);
        Destroy(gameObject, ragdollAliveTime + GetComponent<ParticleSystem>().main.startLifetime.constantMax);
    }

    public void Die()
    {
        GetComponent<ParticleSystem>().Stop();
        triggerStopSystem = true;
        Instantiate(SpellManager.Instance.skeleton, transform.position, Quaternion.identity);
    }
    

    // In event where ragdoll still moves after certain time
    public void SetRagdollChildGameObject(GameObject ragdollChildGameObject)
    {
        this.ragdollChildGameObject = ragdollChildGameObject;
    }
}
