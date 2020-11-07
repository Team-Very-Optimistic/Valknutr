using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public EnemyPack[] availablePacks;
    public float difficultyTarget = 1;

    private bool hasSpawnedEnemies;
    private List<GameObject> enemies = new List<GameObject>();

    public bool IsDone()
    {
        return hasSpawnedEnemies && enemies.All(enemy =>
        {
            if (enemy == null) return true;
            return enemy.GetComponent<HealthScript>()?.dead ?? false;
        });
    }

    public void BeginSpawning(int depth)
    {
        if (hasSpawnedEnemies) return;
        hasSpawnedEnemies = true;
        SpawnEnemies(depth);
    }

    private void SpawnEnemies(int depth)
    {
        if (availablePacks.Length == 0) return;
        float currentDifficulty = 0;
        var toSpawn = new List<EnemyPack>();
        var spawnDensity = DifficultyScalingSystem.GetDensity(depth); 
        
        // Select packs until we meet a difficulty target
        while (currentDifficulty < difficultyTarget * spawnDensity)
        {
            var newPack = Util.RandomItem(availablePacks);
            toSpawn.Add(newPack);
            currentDifficulty += newPack.difficultyRating;
        }
        
        toSpawn.ForEach(pack =>
            pack.SpawnEnemies(RandomSpawnPosition(), depth).ForEach(enemies.Add)
        );
    }

    private Vector3 RandomSpawnPosition()
    {
        return spawnPoints.Length > 0 ? Util.RandomItem(spawnPoints).position : transform.position;
    }

    [ContextMenu("Detect SpawnZones")]
    private void DetectSpawnZones()
    {
        spawnPoints = GetComponentsInChildren<SpawnZone>().Select(i => i.transform).ToArray();
    }
}