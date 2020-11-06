using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelGenerator[] levels;
    public int levelIndex;

    public void Start()
    {
        levelIndex = 0;
    }

    [ContextMenu("Next Level")]
    public void StartNextLevel()
    {
        StartCoroutine(DifficultyScalingSystem.Instance.IncreaseDifficulty(1, 0.0f));
        StartLevel(++levelIndex % levels.Length);
    }

    public static void StartLevel(int levelIndex)
    {
        for (var index = 0; index < Instance.levels.Length; index++)
        {
            if (index == levelIndex) continue;
            var level = Instance.levels[index];
            level.gameObject.SetActive(false);
        }

        if (levelIndex < Instance.levels.Length)
        {
            var level = Instance.levels[levelIndex];
            level.gameObject.SetActive(true);
            var offset = levelIndex * new Vector3(1000, 0, 1000);

            level.transform.position = Vector3.zero;
            level.Generate();
            level.transform.position = offset;

            GameManager.Instance._player.transform.position = offset;
        }
        else
        {
            throw new Exception("Level index out of range");
        }
    }
}