using System;
using UnityEngine;

[Serializable]
public struct RoomPrefabConfig
{
    public GameObject prefab;

    [Range(1, 100)]
    public int weight;

    [Range(0, 10)]
    public int min;

    [Range(0, 10)]
    public int max;

    // [HideInInspector]
    public int currentAmount;
}

[CreateAssetMenu]
public class LevelConfig : ScriptableObject
{
    public RoomPrefabConfig[] roomPrefabs;
    public GameObject bossRoomPrefab;
    public int numberOfRooms = 5;
}