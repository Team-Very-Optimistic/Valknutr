using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class EnemyPack:ScriptableObject
{
    public EnemyPackConfig[] enemies;
    public float difficultyModifier = 1;
    public float difficultyRating = 1;

    public List<GameObject> SpawnEnemies(Vector3 position, int depth)
    {
        var spawned = new List<GameObject>();
        foreach (var enemy in enemies)
        {
            if (!(Random.value < enemy.SpawnChance)) continue;
            for (var i = Mathf.Round(Random.Range(enemy.Count.x, enemy.Count.y)); i > 0 ; i--)
            {
                var spawnedEnemy = Instantiate(enemy.BaseType, position, Quaternion.identity);
                spawnedEnemy.GetComponent<EnemyBehaviourBase>()?.SetDifficulty(DifficultyScalingSystem.GetDifficulty(difficultyModifier * enemy.difficultyModifier, depth));
                spawned.Add(spawnedEnemy);
            }
        }
        return spawned;
    }
}

[Serializable]
public struct EnemyPackConfig
{
    public GameObject BaseType; // prefab for enemy
    [Range(0, 1)]
    public float SpawnChance; // 0.3 = 30% to spawn in pack
    public Vector2 Count; // (Min, Max) count to spawn, uniformly distributed
    public float difficultyModifier; // multiplicative modifier for this specific enemy (stacks with pack modifier)
}