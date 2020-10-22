using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySpells : MonoBehaviour
{
    public GameObject prefab; // This is our prefab object that will be exposed in the inspector
    public RectTransform tooltipPos;
    public List<SpellItem> _spellItems;

    void Start()
    {
    }
    
    void Populate()
    {
        GameObject newObj; // Create GameObject instance

        foreach (var spellItem in _spellItems)
        {
            // Create new instances of our prefab until we've created as many as we specified
            newObj = Instantiate(prefab, transform);
            var uiItem = newObj.GetComponent<UIItem>();
            uiItem._spellItem = spellItem;
        }
    }

    public void RemoveItem(SpellItem spellItem)
    {
        _spellItems.Remove(spellItem);
    }

    public void AddItem(SpellItem spellItem)
    {
        _spellItems.Add(spellItem);
        GameObject newObj = Instantiate(prefab, transform);
        var uiItem = newObj.GetComponent<UIItem>();
        uiItem._spellItem = spellItem;
    }
}