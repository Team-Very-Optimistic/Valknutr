using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void PauseGame()
    {
        UiManager.Instance.PauseGame();
    }

    public void ResumeGame()
    {
        UiManager.Instance.ResumeGame();

    }

    public void GoToMainMenu()
    {
        UiManager.Instance.GoToMainMenu();

    }

    public void QuitGame()
    {
        UiManager.Instance.QuitGame();
    }
}
