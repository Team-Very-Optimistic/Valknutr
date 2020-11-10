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
    public string name;
    public RoomPrefabConfig[] roomPrefabs;
    public GameObject bossRoomPrefab;
    public int numberOfRooms = 5;
    public float densityModifier = 1f;
    public string ambientMusic = "level1";
}