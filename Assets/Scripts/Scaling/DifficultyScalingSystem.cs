using System;
using UnityEngine;


public class DifficultyScalingSystem : Singleton<DifficultyScalingSystem>
{
    /// <summary>
    /// Difficulty level 1 is easier, the higher it is the more difficult it gets
    /// </summary>
    public int difficultyLevel = 1;
    public void Awake()
    {
        EnemyBehaviourBase.OnEnemyStart += ManageDifficulty;
    }

    private void ManageDifficulty(EnemyBehaviourBase enemyBehaviourBase)
    {
        IncreaseEnemyHealth(enemyBehaviourBase);
    }

    public void IncreaseEnemyHealth(EnemyBehaviourBase enemyBehaviourBase)
    {
        var hp = enemyBehaviourBase.GetComponent<HealthScript>();
        hp.SetHealth(hp.maxHealth * difficultyLevel);
    }

    public void IncreaseDifficulty(int amount)
    {
        difficultyLevel += amount;
    }
}
