using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class CraftMenuManager : Singleton<CraftMenuManager>
{
    public UIView craftMenu;
    public UIView selectMenu;

    public DisplaySpells displaySpells;
    public List<UISlot> _itemSlots;
    private const int BaseItemSlotIndex = 3;
    
    #region CraftMenu
    public void DisplayCraftMenu()
    {
        if (craftMenu.IsHidden)
        {
            if (IsSelectMenuDisplayed())
            {
                craftMenu.Show();
                selectMenu.Hide();
            }
            else
            {
                craftMenu.Show();
            }
        }
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
                var slottedItem = slots.GetSlottedSpellItem();
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
            
            spell.AddBaseType(baseSpellItem._spellElement as SpellBase);
            
            List<Sprite> modSprites = new List<Sprite>();
            List<string> modStrings = new List<string>();
            string modStr = "";
            foreach (var mod in mods)
            {
                modStr += mod._spellElement.name + " ";
                spell.AddModifier(mod._spellElement as SpellModifier);
                modSprites.Add(mod._UIsprite);
                modStrings.Add(mod._tooltipMessage);
            }
            spell.name = "spell " + baseSpellItem._spellElement.name + modStr;
            spell._UIsprite = spell.CreateProceduralSprite(baseSpellItem._UIsprite, modSprites);
            spell.CreateTooltip(baseSpellItem._tooltipMessage, modStrings);
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
                var slottedItem = slots.GetSlottedSpellItem();
                Inventory.Instance.Remove(slottedItem);
                slots.Clear();
            }
        }
        
    }
    
    #endregion 
    
    #region SelectMenu
    public void DisplaySelectMenu()
    {
        if (selectMenu.IsHidden)
        {
            if (IsCraftMenuDisplayed())
            {
                craftMenu.Hide();
                selectMenu.Show();
            }
            else
            {
                selectMenu.Show();
            }
        }
        else selectMenu.Hide();
    }

    public bool IsSelectMenuDisplayed()
    {
        return selectMenu.IsVisible;
    }
    
    #endregion

    public bool IsUIDisplayed()
    {
        return IsCraftMenuDisplayed() || IsSelectMenuDisplayed();
    }

    public void SwapUI()
    {
        if (IsCraftMenuDisplayed())
        {
            craftMenu.Hide();
            selectMenu.Show();
        }
        else
        {
            selectMenu.Hide();
            craftMenu.Show();
        }
    }

    public void HideUI()
    {
        craftMenu.Hide();
        selectMenu.Hide();
    }
}
