using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    public GameObject treasurePrefab;
    public PlayerHealth _playerHealth;
    public float healthPickupValue = 2f;
    public float healthPickupDropChance = 0.3f;
    public GameObject shielderShieldPrefab;
    public GameObject cameraRig;

    public QualityManager QualityManager;
    public string levelName;
    
    [HideInInspector]
    public float playTime = 0;
    
    [HideInInspector]
    public bool timerPaused;

    public delegate void PlayerDeathAction();
    public static event PlayerDeathAction OnPlayerDeath;
    
    public delegate void LevelCompleteAction();
    public static event LevelCompleteAction OnLevelComplete;
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
        _playerHealth.OnPlayerDeath += () => OnPlayerDeath?.Invoke();

        //extension method (fluent)
        _weapon = _player.transform.FindDescendentTransform("Weapon").gameObject;
        if (_weapon == null)
        {
            Debug.LogError("No weapon in player in this scene for GameManager");
            return;
        }
    }

    private void Update()
    {
        if (timerPaused) return;
        playTime += Time.deltaTime;
    }

    public ItemDrop SpawnItem(Vector3 position, SpellItem _SpellItem = null, 
        QualityManager.Quality quality = QualityManager.Quality.NotSet, float qualitymodifier = 1f, int level=-1, SpellElement notThis = null)
    {
        level = level == -1 ? DifficultyScalingSystem.GetLevel() : level;
        //Add quality to item
        if (quality == QualityManager.Quality.NotSet)
        {
            quality = QualityManager.GetQuality(DifficultyScalingSystem.GetDifficulty() * qualitymodifier);
        }
        if (_SpellItem == null)
        {
            
            var itemListSpellItems = _itemList._SpellItems;
            for (int i = 0; i < 10; i++)
            {
                _SpellItem = Instantiate(itemListSpellItems[Random.Range(0, itemListSpellItems.Count)]);
                if (_SpellItem._spellElement != notThis && _SpellItem._spellElement.quality <= quality)
                {
                    break;
                }
            }

        }

        if (_SpellItem._spellElement.quality > quality)
        {
            Debug.LogWarning("Spell item quality lower than it should be: " + _SpellItem._spellElement);
        }
        _SpellItem._spellElement = Instantiate(_SpellItem._spellElement); //copies
        QualityManager.RandomizeAndInitProperties(_SpellItem, quality, level);
        
        var itemDrop = Instantiate(itemDropPrefab, position, Quaternion.identity).GetComponent<ItemDrop>();
        itemDrop._spellItem = _SpellItem;
        return itemDrop;
    }
    
    public ItemDrop SpawnBase(Vector3 position, SpellItem _SpellItem = null, 
        QualityManager.Quality quality = QualityManager.Quality.NotSet, SpellElement notThis = null, int level = -1)
    {
        level = level == -1 ? DifficultyScalingSystem.GetLevel() : level;
        //Add quality to item
        if (quality == QualityManager.Quality.NotSet)
        {
            quality = QualityManager.GetQuality(DifficultyScalingSystem.Instance.difficultyLevel);
        }
        if (_SpellItem == null)
        {
            var itemListSpellItems = _itemList._SpellItems;
            for (int i = 0; i < 100; i++)
            {
                var item = itemListSpellItems[Random.Range(0, itemListSpellItems.Count)];
                if(!item.isBaseSpell) continue;
                _SpellItem = Instantiate(item);
                if (_SpellItem._spellElement != notThis && _SpellItem._spellElement.quality <= quality)
                {
                    break;
                }
            }

        }

        if (_SpellItem._spellElement.quality > quality)
        {
            Debug.LogWarning("Spell item quality lower than it should be: " + _SpellItem._spellElement);
        }
        _SpellItem._spellElement = Instantiate(_SpellItem._spellElement); //copies
        QualityManager.RandomizeAndInitProperties(_SpellItem, quality, level);
        
        var itemDrop = Instantiate(itemDropPrefab, position, Quaternion.identity).GetComponent<ItemDrop>();
        itemDrop._spellItem = _SpellItem;
        return itemDrop;
    }

    public ItemDrop SpawnHP(Vector3 position, float healAmount)
    {

        var hp = Instantiate(healthPickupObj, position, Quaternion.identity).GetComponent<ItemDrop>();
        var healthPickup = hp.GetComponent<HealthPickup>();
        healthPickup.healAmount = healAmount;
            
        return hp;
    }

    public static TreasureChest SpawnTreasureChest(Vector3 position, float quality)
    {
        AudioManager.PlaySoundAtPosition("spawnTreasure", position);
        var treasure = Instantiate(Instance.treasurePrefab, position, Quaternion.identity).GetComponent<TreasureChest>();
        treasure.quality = quality;
        // todo: set loot + quality
        return treasure;
    }

    public static void SetGameWin()
    {
        //Display winning screen

        //Disable controls?

        OnLevelComplete?.Invoke();
        //Kill all enemies
        List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        //This code is scary
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    public static void SetGameLose()
    {
        OnPlayerDeath?.Invoke();
    }

    public void HealthPickup(float value)
    {
        _playerHealth.IncreaseMaxHealth(value);
        _playerHealth.IncreaseCurrHealth(value);
    }
    public void AffectPlayerCurrHealth(float value)
    {
        if(value >= 0)
            _playerHealth.IncreaseCurrHealth(value);
        else
        {
            if (-value * _playerHealth.GetDamageMultiplier() >= _playerHealth.currentHealth)
            {

                _playerHealth.ApplyDamage(_playerHealth.currentHealth/_playerHealth.GetDamageMultiplier() - 0.1f);
            }
            else
                _playerHealth.ApplyDamage(-value);
        }
    }

    public static float GetPlayTime()
    {
        return Instance.playTime;
    }
}
