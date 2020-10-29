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

    public void StartNextLevel()
    {
        StartLevel(++levelIndex % levels.Length);
    }

    public static void StartLevel(int levelIndex)
    {
        foreach (var level in Instance.levels)
        {
            level.gameObject.SetActive(false);
        }

        if (levelIndex < Instance.levels.Length)
        {
            var level = Instance.levels[levelIndex];
            level.gameObject.SetActive(true);
            var offset = levelIndex * new Vector3(1000, 0, 1000);

            level.Generate();
            level.transform.position = offset;

            GameManager.Instance._player.transform.position = offset;
        }
    }
}