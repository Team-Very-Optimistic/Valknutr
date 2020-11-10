using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

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

    public void BeginSpawning(int depth, float densityModifer)
    {
        if (hasSpawnedEnemies) return;
        SpawnEnemies(depth, densityModifer);
    }

    private void SpawnEnemies(int depth, float densityModifer)
    {
        StartCoroutine(SpawnEnemiesCoroutine(depth, densityModifer));
    }

    private IEnumerator SpawnEnemiesCoroutine(int depth, float densityModifer)
    {
        if (availablePacks.Length == 0) yield break;
        float currentDifficulty = 0;
        var toSpawn = new List<EnemyPack>();
        var spawnDensity = DifficultyScalingSystem.GetDensity(depth) * densityModifer;
        var minPackDifficulty = availablePacks.Select(pack => pack.difficultyRating).Min();

        var target = difficultyTarget * spawnDensity;

        // Select packs until we meet a difficulty target
        while (currentDifficulty < target)
        {
            var newPack = Util.RandomItem(availablePacks);
            toSpawn.Add(newPack);
            currentDifficulty += newPack.difficultyRating;
            if (currentDifficulty + minPackDifficulty > target) break;
        }

        List<Vector3> randomSpawnPosition = new List<Vector3>();
        int spawnPosIndex = 0;
        for (int i = 0; i < toSpawn.Count; i++)
        {
            Vector3 location = RandomSpawnPosition();
            randomSpawnPosition.Add(location);
            SpawnSequenceManager.Instance.SpawnParticlesAtLocation(randomSpawnPosition[i]);
        }

        SpawnSequenceManager.Instance.StartScreenShakeOnSpawn();

        yield return new WaitForSeconds(SpawnSequenceManager.Instance.GetSpawnTime());

        toSpawn.ForEach(pack =>
           pack.SpawnEnemies(randomSpawnPosition[spawnPosIndex++], depth).ForEach(enemies.Add)
       );

        hasSpawnedEnemies = true;
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