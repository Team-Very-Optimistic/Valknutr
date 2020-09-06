using UnityEngine;



    public class CraftMenuManager : Singleton<CraftMenuManager>
    {
        public GameObject craftMenu;
        public DisplaySpells displaySpells;

        public void AddItem()
        {
            
        }
        public void Display()
        {
            craftMenu.SetActive(!craftMenu.activeSelf);
        }
    }
