using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public GameObject _player;
    public ItemList _itemList;
    public GameObject _weapon;



    public void SpawnItem(Vector3 position, SpellItem _SpellItem = null)
    {
        if (_SpellItem == null)
        {
            var itemListSpellItems = GameManager.Instance._itemList._SpellItems;
            _SpellItem = itemListSpellItems[Random.Range(0, itemListSpellItems.Count)];
        }
        Debug.Log(_SpellItem._itemObject);
        Instantiate(_SpellItem._itemObject, position, _SpellItem._itemObject.transform.rotation);
    }

    public void SetGameWin()
    {
        //Display winning screen

        //Disable controls?

        GameObject.Find("UI").GetComponent<EndGameManagerScript>().DisplayGameWin();

        //Kill all enemies
        List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

    }

    public void GameOver()
    {
        GameObject.Find("UI").GetComponent<EndGameManagerScript>().DisplayGameOver();
    }
}
