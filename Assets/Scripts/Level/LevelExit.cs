using System.Collections;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private bool activated;
    private void OnTriggerStay(Collider other)
    {
        if (activated || other.gameObject != GameManager.Instance._player) return;
        activated = true;
        StartCoroutine(StartNextLevel());
    }

    static IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(5);
        LevelManager.Instance.StartNextLevel();
    }
}
