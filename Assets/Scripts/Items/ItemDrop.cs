using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class ItemDrop : MonoBehaviour
{
    public SpellItem _SpellItem;
    public void OnDestroy()
    {
        GameManager.Instance.SpawnItem(transform.position);
    }


}
