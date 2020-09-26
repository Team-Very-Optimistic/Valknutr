using System;
using UnityEngine;

namespace UI
{
    public class UIInputController : MonoBehaviour
    {
        public PauseMenu pauseMenu;
        private CraftMenuManager uiManager;

        private void Start()
        {
            uiManager = CraftMenuManager.Instance;
        }

        private void Update()
        {
            if (Input.GetButtonDown("CraftMenu"))
            {
                uiManager.DisplayCraftMenu();
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

            //UI Specific controls follow
            if (!uiManager.IsUIDisplayed()) return;
            
            if (Input.GetButtonDown("Cancel"))
            {
                uiManager.HideUI();
            }
            
            if (uiManager.IsCraftMenuDisplayed())
            {
                if (Input.GetButtonDown("Craft"))
                {
                    uiManager.Craft();
                }

                if (Input.GetButtonDown("Back"))
                {
                    uiManager.SwapUI();
                }
            }
            
            if (uiManager.IsQuickCraftMenuDisplayed())
            {
                
                if (Input.GetButtonDown("Next"))
                {
                    uiManager.SwapUI();

                }
            }

        }
    }
}