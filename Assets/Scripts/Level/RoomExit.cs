using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class RoomExit : MonoBehaviour
{
    public bool isConnected = false;
    public bool isLocked = false;
    public bool isOpen = false;
    private Vector3 _originalPosition;
    [SerializeField]
    private RoomExit _connectedExit;
    private Collider _collider;
    private Renderer _renderer;
    private NavMeshObstacle _navMeshObstacle;

    private void Start()
    {
        _originalPosition = transform.position;
        _collider = GetComponentInChildren<Collider>();
        _renderer = GetComponentInChildren<Renderer>();
        _navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        if (isConnected)
        {
            Gizmos.color = isOpen ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        }
    }

    public void Connect(RoomExit other)
    {
        Assert.IsNotNull(other);
        _connectedExit = other;
        other._connectedExit = this;
        isConnected = true;
        other.isConnected = true;
    }

    public void Open()
    {
        // print("opening door " + gameObject.name);
        if (isLocked) return;

        isOpen = true;
        _renderer.enabled = false;
        _collider.enabled = false;
        _navMeshObstacle.enabled = false;
        // print("door unlocked");

        // Open other door
        if (!isConnected || _connectedExit == null || _connectedExit.isOpen) return;
        // print("opening connected");
        _connectedExit.Open();
    }

    public void Close()
    {
        if (isLocked) return;

        isOpen = false;
        _renderer.enabled = true;
        _collider.enabled = true;
        _navMeshObstacle.enabled = true;

        // Close other door
        if (!isConnected || _connectedExit == null || !_connectedExit.isOpen) return;
        _connectedExit.Close();
    }
}