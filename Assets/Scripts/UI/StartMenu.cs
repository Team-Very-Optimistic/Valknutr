using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
