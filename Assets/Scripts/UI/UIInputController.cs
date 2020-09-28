using System;
using UnityEngine;

namespace UI
{
    public class UIInputController : MonoBehaviour
    {
        public PauseMenu pauseMenu;
        public GameObject minimap;
        private void Update()
        {
            if (Input.GetButtonDown("CraftMenu"))
            {
                CraftMenuManager.Instance.Display();
            }

            if (Input.GetButtonDown("Craft") && CraftMenuManager.Instance.IsDisplayed())
            {
                CraftMenuManager.Instance.Craft();
            }
            
            if (Input.GetButtonDown("PauseGame"))
            {
                // todo make pausemenu a singleton?
                if (PauseMenu.isPaused)
                {
                    pauseMenu.ResumeGame();
                }
                else
                {
                    pauseMenu.PauseGame();
                }
            }
            
            if (Input.GetButtonDown("MinimapToggle"))
            {
                minimap.SetActive(!minimap.activeSelf);
            }
        }
    }
}