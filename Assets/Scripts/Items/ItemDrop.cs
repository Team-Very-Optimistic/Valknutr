using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class ItemDrop : MonoBehaviour
{
    public SpellItem _SpellItem;
    public void OnDestroy()
    {
        if (_SpellItem == null)
        {
            var itemListSpellItems = GameManager.Instance._itemList._SpellItems;
            _SpellItem = itemListSpellItems[Random.Range(0, itemListSpellItems.Count)];
        }
        Instantiate(_SpellItem._itemObject, transform.position, _SpellItem._itemObject.transform.rotation);
    }
}
