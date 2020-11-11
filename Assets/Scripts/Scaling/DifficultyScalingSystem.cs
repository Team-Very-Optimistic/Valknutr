using System;
using System.Collections;
using UnityEngine;


public class DifficultyScalingSystem : Singleton<DifficultyScalingSystem>
{
    /// <summary>
    /// Difficulty level 1 is easier, the higher it is the more difficult it gets
    /// </summary>
    public float difficultyLevel = 1;

    public float depthDifficulty = 0.2f;
    public float difficultyScaling = 1.4f;
    public float baseHealthPickupAmount = 2f;

    //public float difficultyIncreaseInterval = 60f;
    
    public void Awake()
    {
        // EnemyBehaviourBase.OnEnemyStart += ManageDifficulty;
        //StartCoroutine(IncreaseDifficulty(1, difficultyIncreaseInterval));
    }

    public void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void ManageDifficulty(EnemyBehaviourBase enemyBehaviourBase)
    {
        IncreaseEnemyHealth(enemyBehaviourBase);
        IncreaseEnemyDamage(enemyBehaviourBase);
    }

    private void IncreaseEnemyDamage(EnemyBehaviourBase enemyBehaviourBase)
    {
        var dps = enemyBehaviourBase.GetComponent<Damage>();
        dps.SetDamage(dps.GetDamage() * difficultyLevel);
    }

    public void IncreaseEnemyHealth(EnemyBehaviourBase enemyBehaviourBase)
    {
        var hp = enemyBehaviourBase.GetComponent<HealthScript>();
        hp.SetHealth(hp.maxHealth * difficultyLevel);
    }

    public IEnumerator _IncreaseDifficulty(float amount, float time)
    {
        yield return new WaitForSeconds(time);
        difficultyLevel += amount;
        GameManager.Instance.healthPickupValue *= difficultyLevel;
        // StartCoroutine(IncreaseDifficulty(amount, time));
    }

    public void IncreaseDifficulty(float amount)
    {
        difficultyLevel += amount;
        GameManager.Instance.healthPickupValue *= difficultyLevel;
    }

    public static float GetDifficulty()
    {
        int? depth = GameManager.Instance.activeRoom.depth;
        return Instance.difficultyLevel + depth.GetValueOrDefault(0) * Instance.depthDifficulty;
    }

    public static int GetLevel()
    {
        return Mathf.RoundToInt(Instance.difficultyLevel);
    }

    public static float GetDensity(int depth)
    {
        return 1;
    }

    public static float GetDifficulty(float enemyDifficultyModifier, int depth)
    {
        return (Instance.difficultyLevel + depth * Instance.depthDifficulty) * enemyDifficultyModifier;
    }

    public static float GetStatsScale(int level)
    {
        return Mathf.Pow(Instance.difficultyScaling, level - 1); 
    }

    public static float GetEnemyStatsScale(float enemyDifficultyModifier, int depth)
    {
        return Mathf.Pow(Instance.difficultyScaling, GetDifficulty(enemyDifficultyModifier, depth) - 1);
    }

    public static float GetHealthPickupHealAmount()
    {
        return Instance.baseHealthPickupAmount * Mathf.Pow(Instance.difficultyScaling, GetLevel() - 1);
    }
}
