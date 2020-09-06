
using System.Collections.Generic;

public class Inventory : Singleton<Inventory>
{
    public List<SpellItem> _spellItems;
    private CraftMenuManager _inventoryUI;

    public void Start()
    {
        _inventoryUI = CraftMenuManager.Instance;
        if(_spellItems == null)
            _spellItems = new List<SpellItem>();
        else
        {
            foreach (var i in _spellItems)
            {
               _inventoryUI.AddItem(i);
            }
        }
    }

    public void Add(SpellItem spellItem)
    {
        _spellItems.Add(spellItem);
        _inventoryUI.AddItem(spellItem);
    }
    
    public void Remove(SpellItem spellItem)
    {
        _spellItems.Remove(spellItem);
        _inventoryUI.RemoveItem(spellItem);
    }


}
