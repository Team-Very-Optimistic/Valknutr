using System.Collections;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private bool activated;
    private void OnTriggerStay(Collider other)
    {
        if (activated || other.gameObject != GameManager.Instance._player) return;
        activated = true;
        StartNextLevel();
    }

    void StartNextLevel()
    {
        LevelManager.StartNextLevel();
    }
}
