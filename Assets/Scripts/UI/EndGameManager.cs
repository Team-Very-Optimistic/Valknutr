using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : Singleton<EndGameManager>
{
    public GameObject gameOverObjects;
    public GameObject gameWinObjects;
    public GameObject fadeBackground;

    private float fadeInMaxTransparency = 0.5f;
    public float fadeInTime;
    private float timeElapsed;
    private bool fadeComplete = true;

    public float gameOverScreenDelay;
    public float fadeBackgroundDelay;

    public List<GameObject> gameUI;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                gameUI.Add(child.gameObject);
            }
        }
    }

    private void Update()
    {
        if(!fadeComplete)
        {
            timeElapsed += Time.deltaTime;

            float percentage = (timeElapsed / fadeInTime);
            Color color = fadeBackground.GetComponent<Image>().color;
            color.a = percentage * fadeInMaxTransparency;
            fadeBackground.GetComponent<Image>().color = color;

            if (percentage >= 1.0f)
            {
                fadeComplete = true;
            }
        }
    }

    public void StartGameOverSequence()
    {
        StartCoroutine(DisplayGameOver(gameOverScreenDelay));
        StartCoroutine(StartFadeInBackground(fadeBackgroundDelay));
    }

    IEnumerator DisplayGameOver(float time)
    {
        yield return new WaitForSeconds(time);
        HideGameUI();
        gameOverObjects.SetActive(true);
    }

    public void DisplayGameWin()
    {
        HideGameUI();
        gameWinObjects.SetActive(true);
    }

    IEnumerator StartFadeInBackground(float time)
    {
        yield return new WaitForSeconds(time);

        fadeComplete = false;
        fadeBackground.SetActive(true);
    }

    private void HideGameUI()
    {
        foreach(GameObject go in gameUI)
        {
            go.SetActive(!go.activeSelf);
        }
    }
}
