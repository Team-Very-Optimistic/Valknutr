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
    public GameObject healthPickupObj;
    private PlayerHealth _playerHealth;
    public float healthPickupValue = 2f;
    public float healthPickupDropChance = 0.3f;

    public QualityManager QualityManager;
    public void Awake()
    {
        _player = GameObject.Find("Player");
        _playerHealth = _player.GetComponent<PlayerHealth>();
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

    public ItemDrop SpawnItem(Vector3 position, SpellItem _SpellItem = null)
    {
        if (_SpellItem == null)
        {
            if (Random.value < healthPickupDropChance)
            {
                var hp = Instantiate(healthPickupObj, position, Quaternion.identity).GetComponent<ItemDrop>();
                return hp;
            }
            var itemListSpellItems = _itemList._SpellItems;
            _SpellItem = itemListSpellItems[Random.Range(0, itemListSpellItems.Count)];
        }
        //Add quality to item
        QualityManager.RandomizeProperties(_SpellItem, QualityManager.GetQuality(DifficultyScalingSystem.Instance.difficultyLevel));
        
        var itemDrop = Instantiate(itemDropPrefab, position, Quaternion.identity).GetComponent<ItemDrop>();
        itemDrop._spellItem = _SpellItem;
        return itemDrop;
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

    public void IncreasePlayerHealth()
    {
        _playerHealth.IncreasePlayerHealth(healthPickupValue);
        
    }
}
