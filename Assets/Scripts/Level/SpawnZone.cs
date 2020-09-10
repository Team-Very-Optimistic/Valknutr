using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    public bool spawnOnStart = true;
    public GameObject[] enemies;
    
    // Start is called before the first frame update
    void Start()
    {
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
        foreach (var enemyPrefab in enemies)
        {
            var facing = Random.insideUnitCircle;
            var rotation = Quaternion.Euler(new Vector3(facing.x, 0, facing.y));
            Instantiate(enemyPrefab, transform.position, rotation, transform);
        }
    }
}
