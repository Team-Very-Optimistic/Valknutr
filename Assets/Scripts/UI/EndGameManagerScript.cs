using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManagerScript : MonoBehaviour
{
    public GameObject gameOverObjects;
    public GameObject gameWinObjects;

    public List<GameObject> gameUI;

    public void DisplayGameOver()
    {
        HideGameUI();
        gameOverObjects.SetActive(!gameOverObjects.activeSelf);
    }

    public void DisplayGameWin()
    {
        HideGameUI();
        gameWinObjects.SetActive(!gameWinObjects.activeSelf);
    }

    private void HideGameUI()
    {
        foreach(GameObject go in gameUI)
        {
            go.SetActive(!go.activeSelf);
        }
    }
}
