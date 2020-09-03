using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct WeightedPrefab
{
    public int weight;
    public GameObject prefab;
}

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    public WeightedPrefab[] roomPrefabs;
    public int numberOfRooms = 5;
    [SerializeField]
    private List<GameObject> _rooms = new List<GameObject>();
    [SerializeField]
    private List<RoomExit> _exits = new List<RoomExit>();
    private bool _isGenerating = false;
    private Random _random = new Random();

    // public void Update()
    // {
    //     if (_isGenerating && _rooms.Count < numberOfRooms)
    //     {
    //         GenerateRoom();
    //     }
    // }asd

    private GameObject GenerateRoom(GameObject roomPrefab, RoomExit sourceExit, Quaternion rotation)
    {
        var newRoomExits = roomPrefab.GetComponent<Room>().exits;
        
        var validExitCandidates = newRoomExits.Where(otherExit => Vector3.Angle(sourceExit.transform.forward, rotation * -otherExit.transform.forward) < 15).ToList();
        if (validExitCandidates.Count == 0) return null;
        var index = Random.Range(0, validExitCandidates.Count);
        var validExit = validExitCandidates.ElementAt(index);
        var validExitName = validExit.name;

        var transform1 = sourceExit.transform;
        var offset = transform1.position + transform1.forward * 0.25f - rotation * validExit.transform.localPosition;
        var newRoom = GenerateRoom(roomPrefab, offset, rotation);
        if (newRoom == null) return newRoom;
        
        var newRoomExit = newRoom.GetComponent<Room>().exits.First(exit => exit.name == validExitName).GetComponent<RoomExit>();
        print(newRoomExit);
        newRoomExit.isConnected = true;
        sourceExit.isConnected = true;

        return newRoom;
    }
    
    private GameObject GenerateRoom(GameObject roomPrefab, Vector3 position, Quaternion rotation)
    {
        var newRoom = Instantiate(roomPrefab, position, rotation, transform);
        
        if (CheckIntersect(newRoom))
        {
            DestroyImmediate(newRoom);
            print("failed");
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
    
    public void GenerateRooms()
    {
        // Generate entrance
        if (_rooms.Count == 0)
        {
            GenerateRoom(roomPrefabs[0].prefab, Vector3.zero, Quaternion.identity);
            return;
        }

        GameObject newRoom = null;
        int iterations = 100;
        
        // select random room type
        var roomType = roomPrefabs[Random.Range(0, roomPrefabs.Length)].prefab;
        while (newRoom == null && iterations-- > 0)
        {
            var rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 4) * 90);
            print(roomType);

            var validExits = _exits.Where(exit => !exit.isConnected);
            print(validExits);
            // var targetExit = validExits.[Random.Range(0, validExits.Length)];
            var roomExits = validExits.ToList();
            var index = Random.Range(0, roomExits.Count());
            var targetExit = roomExits.ElementAt(index);
            print("generating room");
            newRoom = GenerateRoom(roomType, targetExit, rotation);
        }
    }

    private bool CheckIntersect(GameObject newRoom)
    {
        // ensure that new room doesn't intersect with any of the existing rooms
        var newRoomBounds = newRoom.GetComponent<Collider>().bounds;
        return _rooms.Any(room => room != null && room.GetComponent<Collider>().bounds.Intersects(newRoomBounds));
    }

    public void Generate()
    {
        Cleanup();
        _isGenerating = true;
        for (var i = 0; i < numberOfRooms; i++)
        {
            GenerateRooms();

        }
    }

    public void Cleanup()
    {
        print("cleaning up " + _rooms.Count + " rooms");
        foreach (var room in _rooms)
        {
            if (room != null)
                DestroyImmediate(room);
        }

        _rooms.Clear();
        _exits.Clear();
    }
}