using System;
using UnityEngine;

namespace UI
{
    public class UIInputController : Singleton<UIInputController>
    {
        private CraftMenuManager craftMenuManager;
        private UiManager uiManager;
        private ThirdPersonUserControl _controls;
        private void Start()
        {
            craftMenuManager = CraftMenuManager.Instance;
            uiManager = UiManager.Instance;
            _controls = GameManager.Instance._player.GetComponent<ThirdPersonUserControl>();
        }

        private void Update()
        {
            
            if (Input.GetButtonDown("PauseGame"))
            {
                if (!craftMenuManager.IsUIDisplayed())
                    uiManager.TogglePause();
            }

            if (!UiManager.Instance.isPaused)
            {
                if (Input.GetButtonDown("CraftMenu"))
                {
                    craftMenuManager.DisplayCraftMenu();
                }

                if (!craftMenuManager.IsUIDisplayed() && Input.GetButtonDown("SelectMenu"))
                    craftMenuManager.DisplaySelectMenu();

                if (Input.GetButtonDown("MinimapToggle")) uiManager.ToggleMinimap();

                //UI Specific controls follow
                if (!craftMenuManager.IsUIDisplayed())
                {
                    //Enable player controls;
                    if (!_controls.enabled)
                    {
                        _controls.enabled = true;
                    }

                    return;
                }
                else
                {
                    //Disable player controls;
                    if (_controls.enabled)
                    {
                        _controls.enabled = false;
                    }
                }
            }
            

            if (Input.GetButtonDown("Cancel")) craftMenuManager.HideUI();

            if (craftMenuManager.IsCraftMenuDisplayed())
            {
                if (Input.GetButtonDown("Craft")) craftMenuManager.Craft();

                if (Input.GetButtonDown("Back"))
                {
                    craftMenuManager.SwapUI();
                    return;
                }
            }
            
            if (craftMenuManager.IsSelectMenuDisplayed())
            {
                
                if (Input.GetButtonDown("Next")) craftMenuManager.SwapUI();
            }
            
            if (Input.GetButtonUp("SelectMenu"))
            {
                craftMenuManager.HideUI();
                return;
            }
        }
    }
}