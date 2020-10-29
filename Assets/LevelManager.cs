using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelGenerator[] levels;

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
            level.Generate();
            GameManager.Instance._player.transform.position = Vector3.up;
        }
    }
}