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
        ragdollAliveTime -= Time.deltaTime;

        if (ragdollAliveTime <= 0.0f)
        {
            if (!triggerStopSystem)
            {
                GetComponent<ParticleSystem>().Stop();
                triggerStopSystem = true;
            }
        }

        if(ragdollChildGameObject != null)
        {
            this.transform.position = ragdollChildGameObject.transform.position;
        }
    }

    public void SetRagdollAliveTime(float ragdollAliveTime)
    {
        this.ragdollAliveTime = ragdollAliveTime;
        Destroy(this.gameObject, ragdollAliveTime + GetComponent<ParticleSystem>().main.startLifetime.constantMax);
    }

    // In event where ragdoll still moves after certain time
    public void SetRagdollChildGameObject(GameObject ragdollChildGameObject)
    {
        this.ragdollChildGameObject = ragdollChildGameObject;
    }
}
