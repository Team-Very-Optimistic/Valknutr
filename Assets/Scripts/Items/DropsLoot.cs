using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class DropsLoot : MonoBehaviour
{
    public SpellItem _SpellItem;
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
        if (hasDropped) return;
        hasDropped = true;
        pos = transform.position;
        if(GameManager.Instance!= null)
            GameManager.Instance.SpawnItem(pos + Vector3.up * 0.5f, _SpellItem);
    }
}
