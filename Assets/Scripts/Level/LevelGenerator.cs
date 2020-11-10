using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class LevelGenerator : MonoBehaviour
{
    private RoomPrefabConfig[] roomPrefabs;
    private GameObject bossRoomPrefab;
    private int numberOfRooms = 5;
    [SerializeField] private List<GameObject> _rooms = new List<GameObject>();
    [SerializeField] private List<RoomExit> _exits = new List<RoomExit>();
    private NavMeshSurface _navMeshSurface;
    public bool generateOnAwake;
    [Range(0.0001f, 0.01f)] public float noiseOffset = 0.001f;
    public LevelConfig config;
    public GameObject bossRoom;


    private void Awake()
    {
        GameManager.Instance.levelName = config ? config.name : "";
        // name = config.name ?? "";
        _navMeshSurface = GetComponent<NavMeshSurface>();
        print("Generating level");
        // Rebuilds navmesh at start of game to prevent bugs with run-in-editor stuff
        print("Done generating level");
    }

    private void Start()
    {
        if (generateOnAwake) Generate();
        else
            StartCoroutine(rebuildNavMeshDelayed(0.5f));
    }

    /// <summary>
    /// Attempts to generate the specified room connected to the exit
    /// </summary>
    /// <param name="roomPrefab"></param>
    /// <param name="sourceExit"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private GameObject GenerateRoomAtExit(GameObject roomPrefab, RoomExit sourceExit)
    {
        var newRoomExits = roomPrefab.GetComponent<Room>().exits.Shuffle();

        // Select an exit in the new room that faces our chosen exit
        Quaternion rotation;
        GameObject newRoom = null;
        String selectedExitName = "";

        foreach (var exit in newRoomExits)
        {
            var angle = Vector3.Angle(sourceExit.transform.forward, exit.transform.forward);
            rotation = Quaternion.Euler(0, angle, 0);
            var transform1 = sourceExit.transform;
            var offset = transform1.position + noiseOffset * Random.insideUnitSphere - Vector3.Scale(
                Vector3.Scale(rotation * exit.transform.position, Vector3.one), transform.localScale);
            newRoom = GenerateRoomAt(roomPrefab, offset, rotation);
            selectedExitName = exit.name;
            if (newRoom != null) break;
        }

        // var validExitCandidates = newRoomExits.Where(otherExit =>
        //     Vector3.Angle(sourceExit.transform.forward, rotation * -otherExit.transform.forward) < 15).ToList();
        // if (validExitCandidates.Count == 0) return null;
        // var index = Random.Range(0, validExitCandidates.Count);
        // var validExit = validExitCandidates.ElementAt(index);
        // var validExitName = validExit.name;
        //
        // var transform1 = sourceExit.transform;
        // // print(transform1.position);
        // // print(validExit.transform.lossyScale);
        // // print(rotation * validExit.transform.position);
        // var offset = transform1.position + noiseOffset * Random.insideUnitSphere - Vector3.Scale(
        //     Vector3.Scale(rotation * validExit.transform.position, Vector3.one), transform.localScale);
        // var newRoom = GenerateRoomAt(roomPrefab, offset, rotation);
        if (newRoom == null) return null;

        var newRoomExit = newRoom.GetComponent<Room>().exits.First(exit => exit.name == selectedExitName)
            .GetComponent<RoomExit>();

        sourceExit.Connect(newRoomExit);
        return newRoom;
    }

    private GameObject GenerateRoomConnectedTo(GameObject roomPrefab, Room targetRoom)
    {
        GameObject newRoom = null;
        foreach (var exit in targetRoom.exits.Shuffle())
        {
            newRoom = GenerateRoomAtExit(roomPrefab, exit.GetComponent<RoomExit>());
            if (newRoom != null) return newRoom;
        }

        return newRoom;
    }


    /// <summary>
    /// Attempts to generate the specified room at the specified position
    /// Note: Use GenerateRoomAtExit instead
    /// </summary>
    /// <param name="roomPrefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private GameObject GenerateRoomAt(GameObject roomPrefab, Vector3 position, Quaternion rotation)
    {
        var newRoom = Instantiate(roomPrefab, position, rotation, transform);
        newRoom.GetComponent<Room>().densityModifer = config.densityModifier;

        if (CheckIntersect(newRoom))
        {
            DestroyImmediate(newRoom);
            return null;
        }

        var newExits = newRoom.GetComponent<Room>().exits;
        _rooms.Add(newRoom);
        foreach (var newExit in newExits)
        {
            _exits.Add(newExit.GetComponent<RoomExit>());
        }

        return newRoom;
    }

    private RoomExit[] GetExitsByDepth(int depth)
    {
        var exits = new List<RoomExit>();
        _rooms
            .Select(o => o.GetComponent<Room>())
            .Where(room => room.depth == depth)
            .Select(room => room.exits)
            .ToList()
            .ForEach(objects =>
                exits.AddRange(
                    objects.Select(o => o.GetComponent<RoomExit>())));
        return exits.ToArray();
    }

    public void GenerateRoom()
    {
        // Generate entrance
        if (_rooms.Count == 0)
        {
            GenerateRoomAt(roomPrefabs[0].prefab, Vector3.zero, Quaternion.identity);
            return;
        }

        GameObject newRoom = null;
        int iterations = 10;

        // select random room type
        ref var roomTypeConfig = ref ChooseRandomRoom();
        // print(roomTypeConfig.currentAmount);
        var roomType = roomTypeConfig.prefab;
        GameObject targetRoom = null;
        while (newRoom == null && iterations-- > 0)
        {
            // rooms with at least 1 unconnected exit
            var validRooms = _rooms.Where(room =>
                room.GetComponent<Room>().exits.Any(exit => !exit.GetComponent<RoomExit>().isConnected));

            // choose a random room
            targetRoom = Util.RandomItem(validRooms);
            var validExits = targetRoom.GetComponent<Room>().exits
                .Where(exit => !exit.GetComponent<RoomExit>().isConnected);
            var targetExit = Util.RandomItem(validExits).GetComponent<RoomExit>();
            newRoom = GenerateRoomAtExit(roomType, targetExit);
        }

        if (newRoom != null)
        {
            newRoom.GetComponent<Room>().depth = targetRoom.GetComponent<Room>().depth + 1;
            // print(roomTypeConfig.prefab.name + ": " + roomTypeConfig.currentAmount);
            roomTypeConfig.currentAmount += 1;
        }
    }

    /// <summary>
    /// Chooses a random room to generate, subject to weight and min/max considerations
    /// </summary>
    /// <returns></returns>
    private ref RoomPrefabConfig ChooseRandomRoom()
    {
        var n = 10;
        while (n-- > 0)
        {
            var validPrefabs = roomPrefabs.Where(prefab => prefab.max == 0 || prefab.currentAmount < prefab.max)
                .ToArray();
            var mask = roomPrefabs.Select(prefab => prefab.max == 0 || prefab.currentAmount < prefab.max).ToArray();
            var totalWeight = validPrefabs.Sum(prefab => prefab.weight);
            var randomIndex = Random.Range(0, totalWeight);
            for (var i = 0; i < roomPrefabs.Length; i++)
            {
                if (!mask[i]) continue;
                if (randomIndex <= 0)
                {
                    return ref roomPrefabs[i];
                }

                randomIndex -= roomPrefabs[i].weight;
            }

            if (mask[roomPrefabs.Length - 1]) return ref roomPrefabs[roomPrefabs.Length - 1];
        }

        return ref roomPrefabs[0];
    }

    private bool CheckIntersect(GameObject newRoom)
    {
        // ensure that new room doesn't intersect with any of the existing rooms
        var newRoomBounds = newRoom.GetComponent<Collider>().bounds;
        return _rooms.Any(room => room != null && room.GetComponent<Collider>().bounds.Intersects(newRoomBounds));
    }

    public void Generate()
    {
        if (!config) return;
        var startTime = Time.realtimeSinceStartup;
        roomPrefabs = config.roomPrefabs;
        bossRoomPrefab = config.bossRoomPrefab;
        numberOfRooms = config.numberOfRooms;
        if (Application.isPlaying) GameManager.Instance.levelName = config.name ?? "???";

        var n = 10;
        while (n-- > 0)
        {
            Cleanup();
            for (var i = 0; i < numberOfRooms; i++)
            {
                GenerateRoom();
            }

            var satisfiesRoomMinCounts = true;
            foreach (var prefab in roomPrefabs)
            {
                // print(prefab.prefab.name + ": " + prefab.currentAmount);
                if (prefab.currentAmount < prefab.min)
                {
                    satisfiesRoomMinCounts = false;
                }
            }

            if (!satisfiesRoomMinCounts)
            {
                continue;
            }

            try
            {
                PlaceBossRoom();
            }
            catch (GenerationException e)
            {
                print("failed generation");
                continue;
            }

            print($"success: {Time.realtimeSinceStartup - startTime}");
            break;
        }

        if (Application.isPlaying)
            StartCoroutine(rebuildNavMeshDelayed(0.5f));
        else
            RebuildNavMesh();
        HideAllUnconnectedExitIcons();
    }

    private void HideAllUnconnectedExitIcons()
    {
        _exits.ForEach(exit => exit.HideMinimapIcon());
    }

    public void Cleanup()
    {
        print("cleaning up " + _rooms.Count + " rooms");
        for (int i = 0; i < roomPrefabs.Length; i++)
        {
            roomPrefabs[i].currentAmount = 0;
        }

        foreach (var room in _rooms)
        {
            if (room != null)
                DestroyImmediate(room);
        }

        _rooms.Clear();
        _exits.Clear();


        if (Application.isPlaying)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        else
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }

    public void RemoveConnectedExits()
    {
        foreach (var exit in _exits)
        {
            if (exit.isConnected)
            {
                DestroyImmediate(exit.gameObject);
            }
        }
    }

    public void RemoveRoomColliders()
    {
        foreach (var room in _rooms)
        {
            foreach (var component in room.GetComponents<Collider>())
            {
                DestroyImmediate(component);
            }
        }
    }

    private void PlaceBossRoom()
    {
        var success = false;
        var n = 10;

        var sortedRooms = _rooms.Select(room =>
            (room.GetComponent<Room>().depth,
            room
        )).OrderBy(a => -a.depth).ToArray();

        foreach ((_, GameObject go) in sortedRooms)
        {
            bossRoom = GenerateRoomConnectedTo(bossRoomPrefab, go.GetComponent<Room>());
            success = bossRoom != null;
            if (success) break;
        }

        if (!success)
            throw new GenerationException();
    }

    public void RebuildNavMesh()
    {
        var startTime = Time.realtimeSinceStartup;
        if (!_navMeshSurface)
            _navMeshSurface = GetComponent<NavMeshSurface>();
        _navMeshSurface.BuildNavMesh();
        print($"Rebuilding Navmesh: {Time.realtimeSinceStartup - startTime}");
    }

    private IEnumerator rebuildNavMeshDelayed(float time)
    {
        yield return new WaitForSeconds(time);
        RebuildNavMesh();
    }
}

public class GenerationException : Exception
{
}