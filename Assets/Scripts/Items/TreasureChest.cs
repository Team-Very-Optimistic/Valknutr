using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreasureChest : HealthScript
{
    public int numTreasure = 3;
    [SerializeField]
    private float offsetDistance;

    public static bool firstSpawn = true;
    public float quality;

    public override void OnDeath()
    {
        DamageTextManager.SpawnTempWord("Choose one . . .", transform.position + Vector3.up, Color.yellow);
        var direction =  transform.position - GameManager.Instance._player.transform.position;
        direction.y = 0;
        direction = direction.normalized;
        var offset = offsetDistance * new Vector3(-direction.z, 0, direction.x);
        ItemDrop[] itemDrops = new ItemDrop[numTreasure];
        if (firstSpawn)
        {
            firstSpawn = false;
            SpellElement prev = null;
            for (int i = 0; i < numTreasure; i++)
            {
                ItemDrop itemDrop; 

                itemDrop = GameManager.Instance.SpawnBase(transform.position + (i - (numTreasure - 1) / 2) * offset + direction, notThis: prev);
                prev = itemDrop._spellItem._spellElement;
            
                itemDrops[i]  = itemDrop;
            }
            for (int i = 0; i < numTreasure; i++)
            {
                for (int j = 0; j < numTreasure; j++)
                {
                    itemDrops[i].OnPickup  += itemDrops[j].PickupHandler;
                }
            }
            base.OnDeath();
            return;
        }
        int hp = Random.Range(0, numTreasure);
        SpellElement prevDrop = null;
        for (int i = 0; i < numTreasure; i++)
        {
            ItemDrop itemDrop; 
            if (i == hp)
            {
                itemDrop = GameManager.Instance.SpawnHP(transform.position + (i - (numTreasure - 1) / 2) * offset + direction);
            }
            else
            {
                itemDrop = GameManager.Instance.SpawnItem(transform.position + (i - (numTreasure - 1) / 2) * offset + direction, notThis: prevDrop, qualitymodifier: quality);
                prevDrop = itemDrop._spellItem._spellElement;
            }
            itemDrops[i]  = itemDrop;
        }
        for (int i = 0; i < numTreasure; i++)
        {
            for (int j = 0; j < numTreasure; j++)
            {
                itemDrops[i].OnPickup  += itemDrops[j].PickupHandler;
            }
        }
        base.OnDeath();
    }
}