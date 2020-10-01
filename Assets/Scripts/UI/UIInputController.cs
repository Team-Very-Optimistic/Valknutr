using System;
using UnityEngine;

namespace UI
{
    public class UIInputController : MonoBehaviour
    {
        private CraftMenuManager craftMenuManager;
        private UiManager uiManager;

        private void Start()
        {
            craftMenuManager = CraftMenuManager.Instance;
            uiManager = UiManager.Instance;
        }

        private void Update()
        {
            if (Input.GetButtonDown("CraftMenu")) craftMenuManager.DisplayCraftMenu();

            if (!craftMenuManager.IsUIDisplayed() && Input.GetButtonDown("SelectMenu"))
                craftMenuManager.DisplaySelectMenu();

            if (Input.GetButtonDown("PauseGame")) uiManager.TogglePause();
            
            if (Input.GetButtonDown("MinimapToggle")) uiManager.ToggleMinimap();

            //UI Specific controls follow
            if (!craftMenuManager.IsUIDisplayed()) return;
            
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