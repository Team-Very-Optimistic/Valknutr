using System.Collections.Generic;
using UnityEngine;

public class CraftMenuManager : Singleton<CraftMenuManager>
{
    public GameObject craftMenu;
    public DisplaySpells displaySpells;
    public List<UISlot> _itemSlots;

    public void AddItem(SpellItem spellItem)
    {
        displaySpells.AddItem(spellItem);
    }
    
    public void Display()
    {
        craftMenu.SetActive(!craftMenu.activeSelf);
    }

    public void RemoveItem(SpellItem spellItem)
    {
        displaySpells.RemoveItem(spellItem);
    }
}
