using System.Collections.Generic;
using UnityEngine;

public class CraftMenuManager : Singleton<CraftMenuManager>
{
    public GameObject craftMenu;
    public DisplaySpells displaySpells;
    public List<UISlot> _itemSlots;
    private const int BaseItemSlotIndex = 3;
    public void AddItem(SpellItem spellItem)
    {
        displaySpells.AddItem(spellItem);
    }
    
    public void Display()
    {
        craftMenu.SetActive(!craftMenu.activeSelf);
    }

    public bool IsDisplayed()
    {
        return craftMenu.activeSelf;
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
}
