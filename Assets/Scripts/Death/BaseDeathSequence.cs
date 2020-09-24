using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class BaseDeathSequence : MonoBehaviour
{
    //Ragdoll colliders
    public List<Collider> ragdollParts;

    // Start is called before the first frame update
    public virtual void Start()
    {
        GetRagdollParts();
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
            c.isTrigger = false;
        }
    }

    public void CallDestroy(float time)
    {
        Destroy(this.gameObject, time);
    }
}
