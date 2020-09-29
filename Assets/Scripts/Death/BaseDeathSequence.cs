using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
            if (c.gameObject != this.gameObject)
            {
                c.isTrigger = true;
                ragdollParts.Add(c);
            }
        }
    }

    public void TriggerRagdoll()
    {
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<Animator>().avatar = null;

        //Turn on ragdoll
        foreach (Collider c in ragdollParts)
        {
            c.enabled = true;
            c.isTrigger = false;
        }
    }

    public void KnockbackRagdoll()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 forceDirection = (transform.position - player.transform.position).normalized;
        forceDirection.y = 0.1f;
        foreach (Collider c in ragdollParts)
        {
            c.attachedRigidbody.AddForce(forceDirection * RagdollKnockbackForce);
        }
    }

    public void CallDestroy(float time)
    {
        Destroy(this.gameObject, time);
    }
}
