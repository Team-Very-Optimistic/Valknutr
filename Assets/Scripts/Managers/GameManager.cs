using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Debug.Log(_SpellItem._itemObject);
        Instantiate(_SpellItem._itemObject, position, _SpellItem._itemObject.transform.rotation);
    }

    internal static void FindGameObjectWithTag(string v)
    {
        throw new System.NotImplementedException();
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

    public void StartGameOverSequence()
    {
        GameObject.Find("UI").GetComponent<EndGameManagerScript>().StartGameOverSequence();
    }
}
