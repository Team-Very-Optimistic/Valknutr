using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Transform parent;
    private float angularSpeed = 50f;
    private Quaternion rotation;

    private void Start()
    {
        parent = transform.parent;
    }

    private void Update()
    {
        transform.RotateAround(parent.position, Vector3.up, angularSpeed * Time.deltaTime);
        rotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = rotation;
    }

    public void SetSpeed(float speed)
    {
        angularSpeed = speed;
    }
}