using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Collider[] _colliders;
    private Renderer[] _renderers;
    private NavMeshObstacle _navMeshObstacle;
    [HideInInspector]
    public GameObject minimapIcon;

    private void Start()
    {
        _originalPosition = transform.position;
        _colliders = GetComponentsInChildren<Collider>();
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();
        HideMinimapIcon();
    }

    public void HideMinimapIcon()
    {
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.gameObject.SetActive(isConnected);
        }
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
        foreach (var r in _renderers)
        {
            r.enabled = false;
        }
        
        foreach (var c in _colliders)
        {
            c.enabled = false;
        }
        
        if(_navMeshObstacle)
            _navMeshObstacle.enabled = false;

        // _renderer.enabled = false;
        // _collider.enabled = false;
        // if(_navMeshObstacle)
        //     _navMeshObstacle.enabled = false;
    }

    public void Close()
    {
        if (isLocked) return;

        isOpen = false;
        foreach (var r in _renderers)
        {
            r.enabled = true;
        }
        
        foreach (var c in _colliders)
        {
            c.enabled = true;
        }
        
        if(_navMeshObstacle)
            _navMeshObstacle.enabled = true;

        // Close other door
        if (!isConnected || _connectedExit == null || !_connectedExit.isOpen) return;
        _connectedExit.Close();
    }

    [ContextMenu("Generate Minimap Icon")]
    private void GenerateMinimapIcon()
    {
        foreach (var c in GetComponentsInChildren<Collider>())
        {
            var icon = generateMinimapIcon(c);
            icon.transform.parent = transform;
        }

    }
    
    private GameObject generateMinimapIcon(Collider c)
    {
        var bounds = c.bounds;
        var iconPrefab = Resources.Load<GameObject>("MinimapIcon_Exit");

        var minimapIcon = Instantiate(iconPrefab);
        var spriteRenderer = minimapIcon.GetComponent<SpriteRenderer>();
        var spriteWidth = spriteRenderer.bounds.size.x;
        var spriteHeight = spriteRenderer.bounds.size.z;

        var cWidth = bounds.size.x;
        var cHeight = bounds.size.z;

        minimapIcon.transform.localScale = new Vector3(cWidth / spriteWidth, cHeight / spriteHeight, 1);
        minimapIcon.transform.position = bounds.center + Vector3.up * 5;
        return minimapIcon;
    }

    [ContextMenu("Auto center")]
    private void AutoCenter()
    {
        var c =GetComponentInChildren<Collider>();
        var center = c.bounds.center;
        center.y = 0;
        c.transform.position = -center;
    }
}