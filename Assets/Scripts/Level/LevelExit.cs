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
        StartCoroutine(_StartNextLevel());

    }

    static IEnumerator _StartNextLevel()
    {
        UiManager.FadeToBlack(1);
        yield return new WaitForSeconds(1);
        LevelManager.Instance.StartNextLevel();
        UiManager.FadeFromBlack(1);
        UiManager.HideBlackOverlay();
    }
}
