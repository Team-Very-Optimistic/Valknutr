using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class RoomExit : MonoBehaviour
{
    [HideInInspector]
    public bool isConnected = false;
    [HideInInspector]
    public bool isLocked = false;
    [HideInInspector]
    public bool isOpen = false;
    private Vector3 _originalPosition;
    [SerializeField]
    private RoomExit _connectedExit;
    private Collider _collider;
    private Renderer _renderer;
    private NavMeshObstacle _navMeshObstacle;
    [HideInInspector]
    public GameObject minimapIcon;

    private void Start()
    {
        _originalPosition = transform.position;
        _collider = GetComponentInChildren<Collider>();
        _renderer = GetComponentInChildren<Renderer>();
        _navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();
        HideMinimapIcon();
    }

    public void HideMinimapIcon()
    {
        if (!minimapIcon) minimapIcon = GetComponentInChildren<SpriteRenderer>()?.gameObject;
        if(minimapIcon)
            minimapIcon.SetActive(isConnected);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isConnected ? Color.white : Color.gray;
        // Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        if (isConnected)
        {
            Gizmos.color = isOpen ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
            Gizmos.DrawLine(transform.position, _connectedExit.transform.position);
        }
    }

    public void Connect(RoomExit other)
    {
        //Assert.IsNotNull(other);
        _connectedExit = other;
        other._connectedExit = this;
        isConnected = true;
        other.isConnected = true;
    }

    public bool Open()
    {
        if (isLocked) return false;
        var didOpen = !isOpen;
        _Open();
        
        if (!isConnected || _connectedExit == null || _connectedExit.isOpen) return false;
        didOpen = _connectedExit.Open() || didOpen;
        return didOpen;
    }

    private void _Open()
    {
        isOpen = true;
        _renderer.enabled = false;
        _collider.enabled = false;
        if(_navMeshObstacle)
            _navMeshObstacle.enabled = false;
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