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
        return hasSpawnedEnemies && enemies.All(enemy => enemy == null); // todo
    }

    public void BeginSpawning(int depth)
    {
        if (hasSpawnedEnemies) return;
        hasSpawnedEnemies = true;
        SpawnEnemies(DifficultyScalingSystem.Instance.difficultyLevel + (float) depth / DifficultyScalingSystem.Instance.depthDifficulty);
    }

    private void SpawnEnemies(float difficulty)
    {
        if (availablePacks.Length == 0) return;
        float currentDifficulty = 0;
        List<EnemyPack> toSpawn = new List<EnemyPack>();
        print(difficulty);
        // Select packs until we meet a difficulty target
        while (currentDifficulty < difficultyTarget * difficulty)
        {
            var newPack = Util.RandomItem(availablePacks);
            toSpawn.Add(newPack);
            currentDifficulty += newPack.difficultyRating;
        }

        var spawnPosition = spawnPoints.Length > 0 ? spawnPoints[0].position : transform.position;

        toSpawn.ForEach(pack =>
            pack.SpawnEnemies(spawnPosition).ForEach(enemies.Add)
        );
    }

    [ContextMenu("Detect SpawnZones")]
    private void DetectSpawnZones()
    {
        spawnPoints = GetComponentsInChildren<SpawnZone>().Select(i => i.transform).ToArray();
    }
}