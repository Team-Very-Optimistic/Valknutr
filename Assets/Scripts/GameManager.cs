using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject _player;
    public ItemList _itemList;


    public void SpawnItem(Vector3 position, SpellItem _SpellItem = null)
    {
        if (_SpellItem == null)
        {
            var itemListSpellItems = GameManager.Instance._itemList._SpellItems;
            _SpellItem = itemListSpellItems[Random.Range(0, itemListSpellItems.Count)];
        }
        Instantiate(_SpellItem._itemObject, position, _SpellItem._itemObject.transform.rotation);
    }
    
    public void GameOver()
    {
        
    }
}
