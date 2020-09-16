using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    public bool spawnOnStart = false;
    public GameObject[] enemies;
    [SerializeField]
    private bool hasSpawned = false;

    private Room _room;
    
    // Start is called before the first frame update
    void Start()
    {
        _room = GetComponentInParent<Room>();
        if (spawnOnStart)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        // todo: add interval spawning and advanced spawn behaviors
        if (hasSpawned) return;
        hasSpawned = true;
        foreach (var enemyPrefab in enemies)
        {
            var facing = Random.insideUnitCircle;
            var rotation = Quaternion.Euler(new Vector3(facing.x, 0, facing.y));
            var enemy = Instantiate(enemyPrefab, transform.position, rotation);
            var levelKey = enemy.AddComponent<LevelKey>();

            // registers enemy with room
            if (_room != null)
            {
                _room.AddEnemy(enemy);
            }
        }
    }

    public bool IsDone()
    {
        return hasSpawned;
    }

    public void SetActive()
    {
        if (IsDone()) return;
        Spawn();
    }
}
