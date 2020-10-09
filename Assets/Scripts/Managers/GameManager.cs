using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public GameObject _player;
    public ItemList _itemList;
    [HideInInspector]
    public GameObject _weapon;

    public Room activeRoom;
    public GameObject itemDropPrefab;

    public void Awake()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.LogError("No player in this scene for GameManager");
            return;
        }

        SpellBase._player = _player.transform;

        //extension method (fluent)
        _weapon = _player.transform.FindDescendentTransform("Weapon").gameObject;
        if (_weapon == null)
        {
            Debug.LogError("No weapon in player in this scene for GameManager");
            return;
        }
    }

    public void SpawnItem(Vector3 position, SpellItem _SpellItem = null)
    {
        if (_SpellItem == null)
        {
            var itemListSpellItems = _itemList._SpellItems;
            _SpellItem = itemListSpellItems[Random.Range(0, itemListSpellItems.Count)];
        }
        //Debug.Log(_SpellItem._itemObject);
        var itemDrop = Instantiate(itemDropPrefab, position, Quaternion.identity);
        itemDrop.GetComponent<ItemDrop>()._spellItem = _SpellItem;
    }

    public void SetGameWin()
    {
        //Display winning screen

        //Disable controls?

        EndGameManager.Instance.DisplayGameWin();

        //Kill all enemies
        List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        //This code is scary
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

    }

}
