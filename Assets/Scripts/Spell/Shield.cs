using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Transform parent;
    private float angularSpeed = 50f;

    private void Start()
    {
        parent = transform.parent;
    }

    private void Update()
    {
        transform.RotateAround(parent.position, Vector3.up, angularSpeed * Time.deltaTime);
        
    }

    public void SetSpeed(float speed)
    {
        angularSpeed = speed;
    }
}