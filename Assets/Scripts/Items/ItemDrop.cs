using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class ItemDrop : MonoBehaviour
{
    public SpellItem _SpellItem;
    private Vector3 pos;
    public void Start()
    {
        pos = transform.position;
    }

    public void OnDestroy()
    {
        pos = transform.position;
        if(GameManager.Instance!= null)
            GameManager.Instance.SpawnItem(pos);
    }
}
