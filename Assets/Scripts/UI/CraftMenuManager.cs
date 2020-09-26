using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class CraftMenuManager : Singleton<CraftMenuManager>
{
    public UIView craftMenu;
    public UIView quickCraftMenu;

    public DisplaySpells displaySpells;
    public List<UISlot> _itemSlots;
    private const int BaseItemSlotIndex = 3;
    
    #region CraftMenu
    public void DisplayCraftMenu()
    {
        if (craftMenu.IsHidden) SwapUI();
        else craftMenu.Hide();
    }

    public bool IsCraftMenuDisplayed()
    {
        return craftMenu.IsVisible;
    }
    
    public void AddItem(SpellItem spellItem)
    {
        displaySpells.AddItem(spellItem);
    }
    
    public void RemoveItem(SpellItem spellItem)
    {
        displaySpells.RemoveItem(spellItem);
    }

    public void Craft()
    {
        List<SpellItem> mods = new List<SpellItem>();
        SpellItem baseSpellItem = null;
        foreach (var slots in _itemSlots)
        {
            if (slots.IsSlotted())
            {
                var slottedItem = slots.GetSlottedItem();
                if (slots.isBaseSlot)
                {
                    baseSpellItem = slottedItem;
                }
                else
                {
                    mods.Add(slottedItem);
                }
            }
        }

        if (baseSpellItem != null)
        {
            Spell spell = ScriptableObject.CreateInstance<Spell>();
            spell.AddBaseType(baseSpellItem._spellElement as SpellBehavior);
            foreach (var mod in mods)
            {
                spell.AddModifier(mod._spellElement as SpellModifier);
            }
            Inventory.Instance._spells.Add(spell); //crafted
            
        }
        else
        {
            Debug.Log("No base spell for crafting");
            return;
        }
        
        foreach (var slots in _itemSlots)
        {
            if (slots.IsSlotted())
            {
                var slottedItem = slots.GetSlottedItem();
                Inventory.Instance.Remove(slottedItem);
                slots.Clear();
            }
        }
        
    }
    
    #endregion 
    
    #region QuickCraftMenu
    public void DisplayQuickCraftMenu()
    {
        if (quickCraftMenu.IsHidden) SwapUI();
        else quickCraftMenu.Hide();
    }

    public bool IsQuickCraftMenuDisplayed()
    {
        return quickCraftMenu.IsVisible;
    }
    
    #endregion

    public bool IsUIDisplayed()
    {
        return IsCraftMenuDisplayed() || IsQuickCraftMenuDisplayed();
    }

    public void SwapUI()
    {
        if (IsCraftMenuDisplayed())
        {
            UIButtonMessage.Send("Organic");
            craftMenu.Hide();
            quickCraftMenu.Show();
        }
        else
        {
            quickCraftMenu.Hide();
            craftMenu.Show();
        }
    }

    public void HideUI()
    {
        craftMenu.Hide();
        quickCraftMenu.Hide();
    }
}
