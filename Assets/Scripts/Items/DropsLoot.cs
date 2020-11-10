using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class DropsLoot : MonoBehaviour
{
    public GameObject _itemDrop;
    public SpellItem _SpellItem;
    public bool isGameObjectDrop;
    public float dropChance;
    private Vector3 pos;
    private bool hasDropped;
    public void Start()
    {
        pos = transform.position;
    }

    public void OnDestroy()
    {
        DropLoot();
    }

    public void OnDeath()
    {
        DropLoot();
    }

    public void DropLoot()
    {
        if (hasDropped || GameManager.Instance == null) return;
        hasDropped = true;
        pos = transform.position;
        
        if (isGameObjectDrop)
        {
            if (Random.value < dropChance)
                Instantiate(_itemDrop, pos, Quaternion.identity);
        }
        else if(_SpellItem == null)
        {
            GameManager.Instance.SpawnItem(pos + Vector3.up * 0.5f, _SpellItem); 
        }
        else
        {
            if (Random.value < dropChance)
                GameManager.Instance.SpawnHP(pos, DifficultyScalingSystem.GetHealthPickupHealAmount());
        }
    }
}
