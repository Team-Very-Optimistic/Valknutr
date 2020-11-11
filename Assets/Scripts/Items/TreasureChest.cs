using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TreasureChest : HealthScript
{
    public int numTreasure = 3;
    [SerializeField]
    private float offsetDistance;

    public static bool firstSpawn = true;
    public float quality = 1f;

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
                NavMeshHit hit;
                Vector3 finalPosition = transform.position + (i - (numTreasure - 1) / 2) * offset + direction;
                if (NavMesh.SamplePosition(finalPosition, out hit, 1f, 1))
                {
                    finalPosition = hit.position;
                }
                itemDrop = GameManager.Instance.SpawnBase(finalPosition, notThis: prev);
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
                itemDrop = GameManager.Instance.SpawnHP(transform.position + (i - (numTreasure - 1) / 2) * offset + direction, DifficultyScalingSystem.GetHealthPickupHealAmount());
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