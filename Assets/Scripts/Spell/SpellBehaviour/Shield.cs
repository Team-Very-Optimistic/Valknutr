using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Transform parent;
    private float angularSpeed = 50f;
    private Quaternion rotation;
    private Vector3 orgPosition;
    private Transform newParent;

    private void Start()
    {
        parent = transform.parent;
        orgPosition = parent.position - transform.position;
        newParent = new GameObject("shield").transform;
        newParent.transform.position = parent.position;
        transform.SetParent(newParent);
    }

    private void Update()
    {
        newParent.position = parent.position;   
        transform.RotateAround(parent.position, Vector3.up, angularSpeed * Time.deltaTime);
        //rotation = transform.rotation;
    }

    void LateUpdate()
    {
        //transform.rotation = rotation;
    }

    public void SetSpeed(float speed)
    {
        angularSpeed = speed;
    }
}