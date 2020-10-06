using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseDeathSequence : MonoBehaviour
{
    //Ragdoll colliders
    public List<Collider> ragdollParts;
    public float RagdollKnockbackForce = 5000.0f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        GetRagdollParts();

        foreach (Collider c in ragdollParts)
        {
            c.enabled = false;
        }
    }

    private void GetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();

        foreach(Collider c in colliders)
        {
            if (c.gameObject != this.gameObject && c.gameObject.GetComponent<Rigidbody>() != null)
            {
                c.isTrigger = true;
                ragdollParts.Add(c);
            }
        }
    }

    public void TriggerRagdoll()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        GetComponent<Animator>().enabled = false;

        if(GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        //Turn on ragdoll
        foreach (Collider c in ragdollParts)
        {
            c.enabled = true;
            c.isTrigger = false;
            c.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void KnockbackRagdoll()
    {
        //Enable ragdoll knockback in direction of player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 forceDirection = (transform.position - player.transform.position).normalized;
        forceDirection.y = 0.1f;
        foreach (Collider c in ragdollParts)
        {
            c.attachedRigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            c.attachedRigidbody.AddForce(forceDirection * RagdollKnockbackForce);
        }
    }

    public void CallDestroy(float time)
    {
        Destroy(this.gameObject, time);
    }
}
