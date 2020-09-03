using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public bool isConnected = false;
    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        transform.position = isConnected ? _originalPosition + Vector3.down * 10f : _originalPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        if (isConnected)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        }
    }
    
}
