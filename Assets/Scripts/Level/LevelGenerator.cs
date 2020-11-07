using System;
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

    public LevelConfig config;


    private void Awake()
    {
        name = config.name ?? "";
        _navMeshSurface = GetComponent<NavMeshSurface>();
        // Rebuilds navmesh at start of game to prevent bugs with run-in-editor stuff
        RebuildNavMesh();
        if (generateOnAwake) Generate();
    }

    /// <summary>
    /// Attempts to generate the specified room connected to the exit
    /// </summary>
    /// <param name="roomPrefab"></param>
    /// <param name="sourceExit"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private GameObject GenerateRoomAtExit(GameObject roomPrefab, RoomExit sourceExit, Quaternion rotation)
    {
        var newRoomExits = roomPrefab.GetComponent<Room>().exits;

        // Select an exit in the new room that faces our chosen exit
        var validExitCandidates = newRoomExits.Where(otherExit =>
            Vector3.Angle(sourceExit.transform.forward, rotation * -otherExit.transform.forward) < 15).ToList();
        if (validExitCandidates.Count == 0) return null;
        var index = Random.Range(0, validExitCandidates.Count);
        var validExit = validExitCandidates.ElementAt(index);
        var validExitName = validExit.name;

        var transform1 = sourceExit.transform;
        // print(transform1.position);
        // print(validExit.transform.lossyScale);
        // print(rotation * validExit.transform.position);
        var offset = transform1.position - Vector3.Scale(
            Vector3.Scale(rotation * validExit.transform.position, Vector3.one), transform.localScale);
        var newRoom = GenerateRoomAt(roomPrefab, offset, rotation);
        if (newRoom == null) return null;

        var newRoomExit = newRoom.GetComponent<Room>().exits.First(exit => exit.name == validExitName)
            .GetComponent<RoomExit>();

        sourceExit.Connect(newRoomExit);
        return newRoom;
    }

    private GameObject GenerateRoomConnectedTo(GameObject roomPrefab, Room targetRoom)
    {
        GameObject newRoom = null;
        int n = 10;
        while (newRoom == null && n-- > 0)
        {
            var sourceExit = Util.RandomItem(targetRoom.exits).GetComponent<RoomExit>();
            newRoom = GenerateRoomAtExit(roomPrefab, sourceExit, Util.RandomRotationXZ());
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
            var rotation = Util.RandomRotationXZ();

            // rooms with at least 1 unconnected exit
            var validRooms = _rooms.Where(room =>
                room.GetComponent<Room>().exits.Any(exit => !exit.GetComponent<RoomExit>().isConnected));

            // choose a random room
            targetRoom = Util.RandomItem(validRooms);
            var validExits = targetRoom.GetComponent<Room>().exits
                .Where(exit => !exit.GetComponent<RoomExit>().isConnected);
            var targetExit = Util.RandomItem(validExits).GetComponent<RoomExit>();
            newRoom = GenerateRoomAtExit(roomType, targetExit, rotation);
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
        name = config.name;
        UiManager.SetLevelText(name);
        roomPrefabs = config.roomPrefabs;
        bossRoomPrefab = config.bossRoomPrefab;
        numberOfRooms = config.numberOfRooms;

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

            PlaceBossRoom();
            RebuildNavMesh();
            break;
        }

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
        var n = 40;
        var deepestRooms = new List<Room>();
        var depth = -1;
        foreach (var room in _rooms
            .Select(room => room.GetComponent<Room>()))
        {
            if (room.depth > depth)
            {
                deepestRooms.Clear();
                depth = room.depth;
            }
            if (room.depth == depth)
                deepestRooms.Add(room);
        }

        while (n-- > 0 && !success)
        {
            var room = Util.RandomItem(deepestRooms);
            success = GenerateRoomConnectedTo(bossRoomPrefab, room) != null;
        }

        if (!success)
        {
            n = 40;
            while (n-- > 0 && !success)
            {
                var room = Util.RandomItem(_rooms).GetComponent<Room>();
                success = GenerateRoomConnectedTo(bossRoomPrefab, room) != null;
            }
            
        }
        
        if (!success)
            throw new GenerationException();
    }

    public void RebuildNavMesh()
    {
        if (!_navMeshSurface)
            _navMeshSurface = GetComponent<NavMeshSurface>();
        _navMeshSurface.BuildNavMesh();
    }
}

public class GenerationException : Exception
{
}