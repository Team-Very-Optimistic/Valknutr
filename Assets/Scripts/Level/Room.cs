using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    public GameObject levelExit;
    private bool isActive;
    public bool isCleared;
    private bool isPlayerInside;
    private List<GameObject> minimapIcons = new List<GameObject>();
    public int depth;

    // Loot
    public float lootQualityModifier = 1f;
    public bool spawnTreasure = true;

    private Collider roomCollider;

    private Spawner spawner;

    private void Start()
    {
        roomCollider = GetComponent<BoxCollider>();
        spawner = GetComponent<Spawner>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (child.name.Contains("Minimap"))
            {
                minimapIcons.Add(child.gameObject);
            }
        }
        // minimapIcons = transform.FindChildrenByPredicate(transform1 => transform1.GetComponent<SpriteRenderer>())
        //     .Select(transform1 => transform1.gameObject).ToList();
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
        UpdateMinimapIcon(new Color(0.78f, 0.77f, 0f));


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
        UpdateMinimapIcon(new Color(0.16f, 0.32f, 0.17f));
        OpenAllDoors();
        ActivateLevelExit();
        SpawnTreasure();
    }

    private void ActivateLevelExit()
    {
        if (levelExit != null) levelExit.SetActive(true);
    }

    private void UpdateMinimapIcon(Color color)
    {
        // todo
        minimapIcons.ForEach(go =>
        {
            foreach (var spriteRenderer in go.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.color = color;
            }
        });
    }

    private void SpawnTreasure()
    {
        if (spawnTreasure)
        {
            GameManager.SpawnTreasureChest(transform.position + Vector3.up * 5, lootQualityModifier);
            spawnTreasure = false;
        }
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

    [ContextMenu("Remove Minimap Icons")]
    public void RemoveSpriteRenderers()
    {
        // doesnt work
        var minimaps = FindObjectsOfType<SpriteRenderer>();
        foreach (var spriteRenderer in minimaps)
        {
            DestroyImmediate(spriteRenderer.gameObject);
        }

        PrefabUtility.SavePrefabAsset(gameObject);
    }

    [ContextMenu("Generate Minimap Sprite")]
    public void GenerateMinimapSprite()
    {
        var root = new GameObject("MinimapIcons").transform;
        root.parent = transform;

        var colliders = Util.FindChildrenByPredicate(transform, transform1 => transform1.GetComponent<Collider>())
            .Select(c => c.GetComponent<Collider>());

        foreach (var c in colliders)
        {
            if (c.gameObject == gameObject) continue;
            var icon = generateMinimapIcon(c);
            icon.transform.parent = root;
        }
        
        // foreach (var exit in exits)
        // {
        //     foreach (var c in exit.GetComponentsInChildren<Collider>())
        //     {
        //         var exit_icon = generateMinimapIcon(c);
        //         exit_icon.GetComponent<SpriteRenderer>().color = Color.green;
        //         exit_icon.transform.position += Vector3.up;
        //         exit_icon.transform.parent = root;
        //     }
        // }
        // var bounds = GetComponent<Collider>().bounds;
        //
        // var minimapIcon = Instantiate(iconPrefab, root);
        // var spriteRenderer = minimapIcon.GetComponent<SpriteRenderer>();
        // var spriteWidth = spriteRenderer.bounds.size.x;
        // var spriteHeight = spriteRenderer.bounds.size.z;
        //
        // var roomWidth = bounds.size.x;
        // var roomHeight = bounds.size.z;
        //
        // minimapIcon.transform.localScale = new Vector3(roomWidth / spriteWidth, roomHeight / spriteHeight, 1);
        // minimapIcon.transform.position = bounds.center + Vector3.up * 10;

        // PrefabUtility.SavePrefabAsset(gameObject);
    }

    private GameObject generateMinimapIcon(Collider collider)
    {
        var bounds = collider.bounds;
        var iconPrefab = Resources.Load<GameObject>("MinimapIcon_Room");

        var minimapIcon = Instantiate(iconPrefab);
        var spriteRenderer = minimapIcon.GetComponent<SpriteRenderer>();
        var spriteWidth = spriteRenderer.bounds.size.x;
        var spriteHeight = spriteRenderer.bounds.size.z;

        var roomWidth = bounds.size.x;
        var roomHeight = bounds.size.z;

        minimapIcon.transform.localScale = new Vector3(roomWidth / spriteWidth, roomHeight / spriteHeight, 1);
        minimapIcon.transform.position = bounds.center + Vector3.up;
        return minimapIcon;
    }

    [ContextMenu("Detect Exits")]
    public void DetectExits()
    {
        exits = GetComponentsInChildren<RoomExit>().Select(i => i.gameObject).ToArray();
    }
}