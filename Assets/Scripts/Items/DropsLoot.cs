using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class DropsLoot : MonoBehaviour
{
    public GameObject _itemDrop;
    public bool isGameObjectDrop;
    public float dropChance;
    private Vector3 pos;
    public void Start()
    {
        pos = transform.position;
    }

    public void OnDestroy()
    {
        pos = transform.position;
        if (isGameObjectDrop)
        {
            if (Random.value < dropChance)
                Instantiate(_itemDrop, pos, Quaternion.identity);
        }
        else
        {
            if (GameManager.Instance != null)
                GameManager.Instance.SpawnItem(pos);
        }
    }
}
