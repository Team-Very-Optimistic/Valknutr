using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum RoomType
{
    Default,
    Room,
    Corridor,
    Special,
    Boss,
    Treasure
}

[Serializable]
[RequireComponent(typeof(BoxCollider))]
public class Room : MonoBehaviour
{
    public GameObject[] exits;
    private bool isActive;
    private bool isCleared;
    private bool isPlayerInside;
    public int depth;

    // Loot
    public float lootQualityModifier = 1f;
    public bool spawnTreasure = true;

    public GameObject minimapPrefab;
    private Collider roomCollider;

    private Spawner spawner;

    private void Start()
    {
        roomCollider = GetComponent<BoxCollider>();
        spawner = GetComponent<Spawner>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == GameManager.Instance._player)
        {
            var bounds = other.bounds;
            if (roomCollider.bounds.Contains(bounds.min) && roomCollider.bounds.Contains(bounds.max))
                ActivateRoom();
        }
    }

    private void ActivateRoom()
    {
        var currActiveRoom = GameManager.Instance.activeRoom;
        if (currActiveRoom && !currActiveRoom.isCleared) return;
        GameManager.Instance.activeRoom = this;
        isActive = true;
        isPlayerInside = true;

        if (spawner) spawner.BeginSpawning(depth);

        CheckCleared();

        if (!isCleared)
        {
            CloseAllDoors();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void OnDrawGizmos()
    {
        var bounds = GetComponent<Collider>().bounds;
        var color = isCleared ? Color.cyan : (isActive ? Color.green : Color.red);
        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawCube(bounds.center, bounds.size);
    }

    private void Update()
    {
        if (isActive && isCleared && !isPlayerInside)
            isActive = false;
        if (isActive && !isCleared)
            CheckCleared();
    }

    private void CheckCleared()
    {
        isCleared = !spawner || spawner.IsDone();

        if (isCleared)
            OnClear();
    }

    private void OnClear()
    {
        OpenAllDoors();
        if (spawnTreasure)
        {
            SpawnTreasure();
            spawnTreasure = false;
        }
    }

    private void SpawnTreasure()
    {
        GameManager.SpawnTreasureChest(transform.position, lootQualityModifier);
    }

    public void OpenAllDoors()
    {
        var hasOpened = false;
        foreach (var o in exits)
        {
            var exit = o.GetComponent<RoomExit>();
            if (exit == null || !exit.isConnected)
            {
                // print("null exit or not connected");
            }
            else
            {
                // print("opening exit");
                hasOpened = exit.Open() || hasOpened;
            }
        }

        if (hasOpened) AudioManager.PlaySound("doorOpen");
    }

    public void CloseAllDoors()
    {
        foreach (var o in exits)
        {
            var exit = o.GetComponent<RoomExit>();
            if (exit == null || !exit.isConnected)
            {
                // print("null exit or not connected");
            }
            else
            {
                // print("opening exit");
                exit.Close();
            }
        }
    }

    [ContextMenu("Generate Minimap Sprite")]
    public void GenerateMinimapSprite()
    {
        var minimaps = FindObjectsOfType<SpriteRenderer>();

        var bounds = GetComponent<Collider>().bounds;
        print(bounds);
        var minimapIcon = Instantiate(minimapPrefab, transform);
        var spriteRenderer = minimapIcon.GetComponent<SpriteRenderer>();
        var spriteWidth = spriteRenderer.bounds.size.x;
        var spriteHeight = spriteRenderer.bounds.size.z;

        var roomWidth = bounds.size.x;
        var roomHeight = bounds.size.z;

        minimapIcon.transform.localScale = new Vector3(roomWidth / spriteWidth, roomHeight / spriteHeight, 1);
        minimapIcon.transform.position = bounds.center + Vector3.up;
    }

    [ContextMenu("Detect Exits")]
    public void DetectExits()
    {
        exits = GetComponentsInChildren<RoomExit>().Select(i => i.gameObject).ToArray();
    }
}