using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject followObject;

    // Update is called once per frame
    void Update()
    {
        // Used to follow position only (without rotation)

        if(followObject != null)
        {
            transform.position = followObject.transform.position;
        }
    }

    public void SetFollowObject(GameObject followObject)
    {
        this.followObject = followObject;
    }
}
